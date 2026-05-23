using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using PhoenixmlDb.XQuery;
using PhoenixmlDb.XQuery.Execution;
using XQueryMcpServer.Models;

namespace XQueryMcpServer;

[McpServerToolType]
public static class CompileTools
{
    // Cached compilation result — stores the query text so we can re-create
    // a fresh QueryEngine per Run (engines are not thread-safe across executions
    // that share state). The ExecutionPlan itself is safe to reuse across calls.
    private sealed record CachedQuery(string QueryText, ExecutionPlan Plan, string? BaseUri);

    private static readonly ConcurrentDictionary<string, CachedQuery> Cache = new();

    [McpServerTool(Name = "xquery_compile"), Description(
        "Compile an XQuery expression and return a reusable handle for xquery_run. " +
        "The same query always produces the same handle (SHA256-based) — calling compile twice is idempotent. " +
        "Cache is process-lifetime.")]
    public static string Compile(
        [Description("XQuery expression to compile (may include a full prolog)")] string query)
    {
        var handle = Convert.ToHexString(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(query)))[..16];

        if (Cache.ContainsKey(handle))
            return JsonSerializer.Serialize(new { ok = true, handle }, XQueryErrorMapper.JsonOpts);

        try
        {
            var engine = new QueryEngine();
            var compile = engine.Compile(query);
            if (!compile.Success)
            {
                var errs = XQueryErrorMapper.AnalysisErrorsToQueryErrors(compile.Errors, query);
                return JsonSerializer.Serialize(QueryResult.Failure(errs), XQueryErrorMapper.JsonOpts);
            }
            Cache[handle] = new CachedQuery(query, compile.ExecutionPlan!, compile.BaseUri);
            return JsonSerializer.Serialize(new { ok = true, handle }, XQueryErrorMapper.JsonOpts);
        }
        catch (PhoenixmlDb.XQuery.Parser.XQueryParseException parseEx)
        {
            var errs = XQueryErrorMapper.ParseErrorsToQueryErrors(parseEx.Errors, query);
            return JsonSerializer.Serialize(QueryResult.Failure(errs), XQueryErrorMapper.JsonOpts);
        }
    }

    [McpServerTool(Name = "xquery_run"), Description(
        "Execute a previously compiled XQuery (from xquery_compile) against optional XML input with optional external variable bindings. " +
        "Returns the same result shape as xquery_evaluate. Each call is isolated — variable bindings do not leak between calls.")]
    public static async Task<string> Run(
        [Description("Handle returned by xquery_compile")] string handle,
        [Description("Optional XML input document")] string? inputXml = null,
        [Description(
            "Optional JSON object mapping external variable local name → value, e.g. {\"x\": 41, \"name\": \"world\"}. " +
            "Strings bind as xs:string, numbers as xs:double, booleans as xs:boolean. " +
            "Only local names are supported (no namespace prefix). " +
            "Variables must be declared in the query prolog as 'declare variable $name external;'.")] string? variables = null)
    {
        if (!Cache.TryGetValue(handle, out var cached))
        {
            var err = new QueryError("XMCP0001",
                $"Unknown query handle '{handle}'. Call xquery_compile first.",
                null, null, null, null);
            return JsonSerializer.Serialize(QueryResult.Failure(new[] { err }), XQueryErrorMapper.JsonOpts);
        }

        if (!string.IsNullOrEmpty(variables))
        {
            try { JsonDocument.Parse(variables).Dispose(); }
            catch (JsonException ex)
            {
                var err = new QueryError("XMCP0002",
                    $"Invalid variables JSON: {ex.Message}", null, null, null, null);
                return JsonSerializer.Serialize(QueryResult.Failure(new[] { err }), XQueryErrorMapper.JsonOpts);
            }
        }

        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var store = new XdmDocumentStore();
            object? doc = null;
            if (!string.IsNullOrEmpty(inputXml))
                doc = store.LoadFromString(inputXml);

            var engine = new QueryEngine(nodeProvider: store, documentResolver: store);
            using var ctx = engine.CreateContext(
                initialContextItem: doc,
                staticBaseUri: cached.BaseUri);

            if (doc != null)
                ctx.SetExternalVariable("input", doc);

            XQueryErrorMapper.BindVariables(ctx, variables);

            var sb = new System.Text.StringBuilder();
            await foreach (var item in cached.Plan.ExecuteAsync(ctx).ConfigureAwait(false))
            {
                sb.Append(XQueryResultSerializer.Serialize(item, store));
            }
            sw.Stop();

            var value = sb.ToString();
            var isEmpty = string.IsNullOrEmpty(value);
            return JsonSerializer.Serialize(
                QueryResult.Success(
                    value: isEmpty ? null : value,
                    count: isEmpty ? 0 : (int?)null,
                    elapsedMs: sw.ElapsedMilliseconds),
                XQueryErrorMapper.JsonOpts);
        }
        catch (XQueryRuntimeException ex)
        {
            var err = new QueryError(ex.ErrorCode, ex.Message, null, null, null, null);
            return JsonSerializer.Serialize(QueryResult.Failure(new[] { err }), XQueryErrorMapper.JsonOpts);
        }
        catch (PhoenixmlDb.XQuery.Functions.XQueryException ex)
        {
            var err = new QueryError(ex.ErrorCode, ex.Message, null, null, null, null);
            return JsonSerializer.Serialize(QueryResult.Failure(new[] { err }), XQueryErrorMapper.JsonOpts);
        }
    }
}

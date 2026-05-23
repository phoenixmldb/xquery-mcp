using System.ComponentModel;
using System.Text;
using System.Text.Json;
using ModelContextProtocol.Server;
using PhoenixmlDb.XQuery;
using PhoenixmlDb.XQuery.Execution;
using XQueryMcpServer.Models;

namespace XQueryMcpServer;

[McpServerToolType]
public static class ExecutionTools
{
    [McpServerTool(Name = "xquery_evaluate"), Description(
        "Execute an XQuery expression. Optionally provide XML input and external variable bindings. " +
        "Returns a JSON result with 'ok', 'value', 'count', 'elapsedMs', or 'errors' with code+location.")]
    public static async Task<string> Evaluate(
        [Description("XQuery expression to execute")] string query,
        [Description("Optional XML input document")] string? inputXml = null,
        [Description(
            "Optional JSON object mapping external variable local name → value, e.g. {\"name\": \"world\", \"count\": 42}. " +
            "Strings bind as xs:string, numbers as xs:double, booleans as xs:boolean. " +
            "Only local names are supported (no namespace prefix). " +
            "Variables must be declared in the query prolog as 'declare variable $name external;'.")] string? variables = null)
    {
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
        // Pre-validate via QueryEngine.Compile so we can capture structured errors with locations.
        // Note: Compile throws XQueryParseException for syntax errors (before returning Success=false).
        // Semantic/type errors come back as Success=false with AnalysisError entries.
        QueryCompilationResult? compilationResult = null;
        try
        {
            var engine = new QueryEngine();
            compilationResult = engine.Compile(query);
            if (!compilationResult.Success)
            {
                var errs = XQueryErrorMapper.AnalysisErrorsToQueryErrors(compilationResult.Errors, query);
                return JsonSerializer.Serialize(QueryResult.Failure(errs), XQueryErrorMapper.JsonOpts);
            }
        }
        catch (PhoenixmlDb.XQuery.Parser.XQueryParseException parseEx)
        {
            var errs = XQueryErrorMapper.ParseErrorsToQueryErrors(parseEx.Errors, query);
            return JsonSerializer.Serialize(QueryResult.Failure(errs), XQueryErrorMapper.JsonOpts);
        }

        try
        {
            // If variables were provided, drop down to the lower-level API to bind them.
            if (!string.IsNullOrEmpty(variables))
            {
                var store = new XdmDocumentStore();
                object? doc = null;
                if (!string.IsNullOrEmpty(inputXml))
                    doc = store.LoadFromString(inputXml);

                var engine = new QueryEngine(nodeProvider: store, documentResolver: store);
                using var ctx = engine.CreateContext(
                    initialContextItem: doc,
                    staticBaseUri: compilationResult!.BaseUri);

                if (doc != null)
                    ctx.SetExternalVariable("input", doc);

                XQueryErrorMapper.BindVariables(ctx, variables);

                var sb = new StringBuilder();
                await foreach (var item in compilationResult.ExecutionPlan!.ExecuteAsync(ctx).ConfigureAwait(false))
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

            // Fast path: no variables — use the facade.
            var facade = new XQueryFacade();
            var result = await facade.EvaluateAsync(query, inputXml);
            sw.Stop();
            var resultIsEmpty = string.IsNullOrEmpty(result);
            return JsonSerializer.Serialize(
                QueryResult.Success(
                    value: resultIsEmpty ? null : result,
                    count: resultIsEmpty ? 0 : (int?)null,
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

    [McpServerTool(Name = "xquery_test"), Description(
        "Run a query and assert the result equals an expected value. For XML results, compares via XDocument.DeepEquals (canonical, ignores whitespace). For atomic results, compares as strings. " +
        "Returns { passed, actual?, expected?, diff? }. " +
        "Use this for red/green TDD on XQuery.")]
    public static async Task<string> Test(
        [Description("XQuery expression")] string query,
        [Description("Optional XML input document")] string? inputXml,
        [Description("Expected result — XML if it parses as XML, otherwise compared as a string")] string expected,
        [Description("Optional JSON object of external variables: {\"name\": value}")] string? variables = null)
    {
        var evalJson = await Evaluate(query, inputXml, variables);
        using var doc = JsonDocument.Parse(evalJson);
        if (!doc.RootElement.GetProperty("ok").GetBoolean())
            return JsonSerializer.Serialize(new
            {
                passed = false,
                error = evalJson
            }, XQueryErrorMapper.JsonOpts);

        var actual = doc.RootElement.TryGetProperty("value", out var v) ? v.GetString() ?? "" : "";
        bool passed;
        string? diff = null;

        try
        {
            var da = System.Xml.Linq.XDocument.Parse("<r>" + actual + "</r>",
                System.Xml.Linq.LoadOptions.None);
            var db = System.Xml.Linq.XDocument.Parse("<r>" + expected + "</r>",
                System.Xml.Linq.LoadOptions.None);
            passed = System.Xml.Linq.XNode.DeepEquals(da, db);
        }
        catch (System.Xml.XmlException)
        {
            passed = actual.Trim() == expected.Trim();
        }

        if (!passed)
            diff = SimpleDiff(actual, expected);

        return JsonSerializer.Serialize(new
        {
            passed,
            actual,
            expected,
            diff
        }, XQueryErrorMapper.JsonOpts);
    }

    private static string SimpleDiff(string a, string b)
    {
        var la = a.Split('\n');
        var lb = b.Split('\n');
        var sb = new StringBuilder();
        var max = Math.Max(la.Length, lb.Length);
        for (var i = 0; i < max; i++)
        {
            var actualLine = i < la.Length ? la[i] : "";
            var expectedLine = i < lb.Length ? lb[i] : "";
            if (actualLine != expectedLine)
            {
                sb.AppendLine($"- {expectedLine}");
                sb.AppendLine($"+ {actualLine}");
            }
        }
        return sb.ToString();
    }

    [McpServerTool(Name = "xquery_validate"), Description(
        "Validate an XQuery expression without executing it. Returns a JSON result with 'ok' true on success, or 'errors' with code/line/column/snippet on failure.")]
    public static string Validate(
        [Description("XQuery expression to validate")] string query)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var engine = new QueryEngine();
            var compile = engine.Compile(query);
            sw.Stop();
            if (compile.Success)
                return JsonSerializer.Serialize(
                    QueryResult.Success(value: null, count: null, elapsedMs: sw.ElapsedMilliseconds),
                    XQueryErrorMapper.JsonOpts);

            var errs = XQueryErrorMapper.AnalysisErrorsToQueryErrors(compile.Errors, query);
            return JsonSerializer.Serialize(QueryResult.Failure(errs), XQueryErrorMapper.JsonOpts);
        }
        catch (PhoenixmlDb.XQuery.Parser.XQueryParseException parseEx)
        {
            var errs = XQueryErrorMapper.ParseErrorsToQueryErrors(parseEx.Errors, query);
            return JsonSerializer.Serialize(QueryResult.Failure(errs), XQueryErrorMapper.JsonOpts);
        }
    }

    [McpServerTool(Name = "xquery_explain_error"), Description(
        "Explain an XQuery/XPath error code. Returns the spec definition, common causes, and fix suggestions.")]
    public static string ExplainError(
        SpecIndex index,
        [Description("Error code to explain (e.g., 'XPST0003', 'FORG0001')")] string errorCode)
    {
        var entries = index.LookupByCategory("error-code");
        var entry = entries.FirstOrDefault(e =>
            e.Name.Equals(errorCode, StringComparison.OrdinalIgnoreCase));

        if (entry != null)
        {
            var sb = new StringBuilder();
            sb.AppendLine(entry.Content.Trim());
            if (entry.SpecUrl != null)
            {
                sb.AppendLine();
                sb.AppendLine($"Spec: {entry.SpecUrl}");
            }
            return sb.ToString();
        }

        // Provide generic guidance based on error code prefix
        var prefix = errorCode.Length >= 4 ? errorCode[..4] : errorCode;
        var guidance = prefix switch
        {
            "XPST" => "Static error — detected at compile time. Check syntax, names, and types.",
            "XPTY" => "Type error — an expression has the wrong type. Check type annotations and conversions.",
            "XPDY" => "Dynamic error — occurs during evaluation. Check runtime values and context.",
            "FORG" => "Function argument error — wrong type or value passed to a function.",
            "FODC" => "Document/collection error — problem loading or accessing a document.",
            "FOER" => "Error raised by fn:error().",
            "FOAR" => "Arithmetic error — division by zero or numeric overflow.",
            "FORX" => "Regular expression error — invalid regex pattern or flags.",
            "FOTY" => "Type error in a function — invalid argument type.",
            "XQDY" => "XQuery dynamic error — occurs during query evaluation.",
            "FOJS" => "JSON-related error — invalid JSON input or processing.",
            "FOAY" => "Array error — index out of bounds or invalid array operation.",
            _ => "Unknown error category."
        };

        return $"Error code '{errorCode}' is not in the spec database.\n\nCategory guidance: {guidance}\n\nUse xquery_search to find related information.";
    }

    [McpServerTool(Name = "xpath_evaluate"), Description(
        "Evaluate an XPath expression against XML input. Returns a JSON result with 'ok', 'value', 'count', 'elapsedMs', or 'errors' with code+location.")]
    public static async Task<string> EvaluateXPath(
        [Description("XPath expression to evaluate")] string xpath,
        [Description("XML document to evaluate against")] string xml)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        // Pre-validate via QueryEngine.Compile to capture structured errors with locations.
        try
        {
            var engine = new QueryEngine();
            var compile = engine.Compile(xpath);
            if (!compile.Success)
            {
                var errs = XQueryErrorMapper.AnalysisErrorsToQueryErrors(compile.Errors, xpath);
                return JsonSerializer.Serialize(QueryResult.Failure(errs), XQueryErrorMapper.JsonOpts);
            }
        }
        catch (PhoenixmlDb.XQuery.Parser.XQueryParseException parseEx)
        {
            var errs = XQueryErrorMapper.ParseErrorsToQueryErrors(parseEx.Errors, xpath);
            return JsonSerializer.Serialize(QueryResult.Failure(errs), XQueryErrorMapper.JsonOpts);
        }

        try
        {
            var facade = new XQueryFacade();
            var value = await facade.EvaluateAsync(xpath, xml);
            sw.Stop();
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

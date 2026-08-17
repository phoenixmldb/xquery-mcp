using System.Text.Json;
using System.Text.Json.Serialization;
using PhoenixmlDb.XQuery;
using PhoenixmlDb.XQuery.Execution;
using XQueryMcpServer.Models;

namespace XQueryMcpServer;

internal static class XQueryErrorMapper
{
    internal static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    // Extract "XPST0003" from messages like "XPST0003: mismatched input ..."
    internal static string ExtractErrorCode(string message, string fallback)
    {
        if (message.Length >= 8 && message[0] == 'X')
        {
            var colonIdx = message.IndexOf(':');
            if (colonIdx == 8 || colonIdx == 9)
                return message[..colonIdx];
        }
        return fallback;
    }

    internal static string? SourceSnippet(string source, int? line)
    {
        if (line is null || line < 1) return null;
        var lines = source.Split('\n');
        if (line.Value > lines.Length) return null;
        return lines[line.Value - 1];
    }

    internal static QueryError[] ParseErrorsToQueryErrors(
        System.Collections.Generic.IEnumerable<PhoenixmlDb.XQuery.Parser.ParseError> errors,
        string query)
    {
        return errors.Select(e => new QueryError(
            ExtractErrorCode(e.Message, "XPST0003"),
            e.Message,
            e.Line,
            e.Column,
            SourceSnippet(query, e.Line),
            null)).ToArray();
    }

    internal static QueryError[] AnalysisErrorsToQueryErrors(
        System.Collections.Generic.IEnumerable<PhoenixmlDb.XQuery.Analysis.AnalysisError> errors,
        string query)
    {
        return errors.Select(e => new QueryError(
            e.Code ?? "XQST0000",
            e.Message,
            e.Location?.Line,
            e.Location?.Column,
            SourceSnippet(query, e.Location?.Line),
            null)).ToArray();
    }

    /// <summary>
    /// Parses <paramref name="variablesJson"/> (a JSON object) and sets each property as an
    /// external variable on the given <see cref="QueryExecutionContext"/> via
    /// <see cref="QueryExecutionContext.SetExternalVariable(string, object?)"/>.
    /// JSON strings → <c>xs:string</c>, numbers → <c>double</c>, booleans → <c>bool</c>,
    /// null → null, arrays/objects → raw JSON text.
    /// </summary>
    /// <remarks>
    /// Values are bound as raw CLR primitives, not wrapped as <c>XdmValue</c>. That is
    /// forced: the arithmetic operators reach the bound value through
    /// <c>Convert.ToDouble(object)</c>, which throws <see cref="InvalidCastException"/> on an
    /// XdmValue. See the note in <c>CompileToolsTests</c> on the resulting serialization defect.
    /// </remarks>
    /// <param name="ctx">The execution context to bind variables on.</param>
    /// <param name="variablesJson">
    /// Optional JSON object mapping variable local name → value.
    /// Only local names are supported (no namespace prefix). Example: <c>{"name": "world", "count": 42}</c>.
    /// </param>
    internal static void BindVariables(QueryExecutionContext ctx, string? variablesJson)
    {
        if (string.IsNullOrWhiteSpace(variablesJson)) return;
        using var doc = JsonDocument.Parse(variablesJson);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
            throw new ArgumentException(
                $"variables must be a JSON object, got {doc.RootElement.ValueKind}",
                nameof(variablesJson));
        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            object? value = prop.Value.ValueKind switch
            {
                JsonValueKind.String => prop.Value.GetString()!,
                JsonValueKind.Number => prop.Value.GetDouble(),
                JsonValueKind.True or JsonValueKind.False => prop.Value.GetBoolean(),
                JsonValueKind.Null => null,
                _ => prop.Value.GetRawText()  // arrays/objects passed as JSON text
            };
            ctx.SetExternalVariable(prop.Name, value);
        }
    }
}

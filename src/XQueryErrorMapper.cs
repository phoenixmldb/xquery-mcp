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
                // An integral JSON number binds as xs:integer, not xs:double. JSON draws no
                // such distinction, but the two are very different downstream: under the
                // adaptive output method xs:double serializes in exponential form, so
                // {"x": 41} with $x + 1 returned "4.2e1" — correct per the serialization spec
                // (QT3 Serialization-adaptive-44 pins xs:double(1e0) as "1.0e0"), and useless
                // as a tool result. A caller writing 41 means the integer 41.
                //
                // fn:parse-json maps every JSON number to xs:double, but that governs parsing
                // a JSON *document*, not binding external variables, and the ergonomics here
                // point the other way. Non-integral values still bind as xs:double.
                // (object) matters: without it the conditional's natural type is double —
                // long widens to double implicitly, double does not narrow to long — so the
                // integral branch would be converted straight back to the double it exists to
                // avoid, silently.
                JsonValueKind.Number => prop.Value.TryGetInt64(out var i64)
                    ? (object)i64
                    : prop.Value.GetDouble(),
                JsonValueKind.True or JsonValueKind.False => prop.Value.GetBoolean(),
                JsonValueKind.Null => null,
                _ => prop.Value.GetRawText()  // arrays/objects passed as JSON text
            };
            ctx.SetExternalVariable(prop.Name, value);
        }
    }
}

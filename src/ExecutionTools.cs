using System.ComponentModel;
using System.Text;
using ModelContextProtocol.Server;
using PhoenixmlDb.XQuery;
using PhoenixmlDb.XQuery.Execution;

namespace XQueryMcpServer;

[McpServerToolType]
public static class ExecutionTools
{
    [McpServerTool(Name = "xquery_evaluate"), Description(
        "Execute an XQuery expression. Optionally provide XML input. Returns the query results.")]
    public static async Task<string> Evaluate(
        [Description("XQuery expression to execute")] string query,
        [Description("Optional XML input document")] string? inputXml = null)
    {
        try
        {
            var facade = new XQueryFacade();
            var result = await facade.EvaluateAsync(query, inputXml);
            return string.IsNullOrEmpty(result) ? "(empty sequence)" : result;
        }
        catch (Exception ex)
        {
            return $"Query error: {ex.Message}";
        }
    }

    [McpServerTool(Name = "xquery_validate"), Description(
        "Validate an XQuery expression without executing it. Returns compilation errors with line numbers.")]
    public static string Validate(
        [Description("XQuery expression to validate")] string query)
    {
        try
        {
            var engine = new QueryEngine();
            var result = engine.Compile(query);
            if (result.Success)
                return "Valid: query compiled successfully.";

            var sb = new StringBuilder("Errors:\n");
            foreach (var error in result.Errors)
            {
                var location = error.Location != null
                    ? $"Line {error.Location.Line}, Col {error.Location.Column}"
                    : "Unknown location";
                sb.AppendLine($"  [{error.Code}] {location}: {error.Message}");
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Validation error: {ex.Message}";
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
        "Evaluate an XPath expression against XML input. Returns the result.")]
    public static async Task<string> EvaluateXPath(
        [Description("XPath expression to evaluate")] string xpath,
        [Description("XML document to evaluate against")] string xml)
    {
        try
        {
            var facade = new XQueryFacade();
            var result = await facade.EvaluateAsync(xpath, xml);
            return string.IsNullOrEmpty(result) ? "(empty sequence)" : result;
        }
        catch (Exception ex)
        {
            return $"XPath error: {ex.Message}";
        }
    }
}

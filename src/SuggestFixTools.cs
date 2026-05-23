using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace XQueryMcpServer;

[McpServerToolType]
public static class SuggestFixTools
{
    [McpServerTool(Name = "xquery_suggest_fix"), Description(
        "Given an XQuery error code (and optionally the source snippet near the error), " +
        "return a spec-grounded suggestion for fixing it plus a link to the relevant spec section. " +
        "Backed by curated rules for the most common XQuery 3.1/4.0 errors. " +
        "Returns { ok, code, suggestion?, specUrl?, message? }.")]
    public static string Suggest(
        [Description("Error code (e.g., XPST0003, XPTY0004, FORG0001)")] string code,
        [Description("Optional source snippet near the error")] string? snippet = null,
        [Description("Optional 1-based line number")] int? line = null,
        [Description("Optional 1-based column number")] int? column = null)
    {
        if (Rules.TryGetValue(code.ToUpperInvariant(), out var rule))
            return JsonSerializer.Serialize(new
            {
                ok = true,
                code = code.ToUpperInvariant(),
                suggestion = rule.Suggestion,
                specUrl = rule.SpecUrl
            }, XQueryErrorMapper.JsonOpts);

        return JsonSerializer.Serialize(new
        {
            ok = false,
            code,
            message = $"No fix rule for '{code}'. Try xquery_explain_error for general guidance."
        }, XQueryErrorMapper.JsonOpts);
    }

    private static readonly Dictionary<string, (string Suggestion, string SpecUrl)> Rules = new()
    {
        ["XPST0003"] = (
            "Syntax error in the XQuery expression. Check parentheses balance, missing return clause in FLWOR, " +
            "incomplete expressions before a comma, or invalid characters in identifiers. " +
            "Run xquery_validate for precise line/column.",
            "https://www.w3.org/TR/xquery-31/#id-grammar"),
        ["XPST0008"] = (
            "Undeclared variable. Check spelling, scope (FLWOR variables only visible inside the clause that introduces them), " +
            "and that the namespace prefix on the variable name is bound.",
            "https://www.w3.org/TR/xquery-31/#id-variables"),
        ["XPST0017"] = (
            "Function call doesn't match any declared function signature. " +
            "Check the arity (number of arguments), function name spelling, and namespace prefix. " +
            "Use xquery_lookup_function to verify the signature.",
            "https://www.w3.org/TR/xquery-31/#id-function-calls"),
        ["XPTY0004"] = (
            "Type mismatch — an expression's static type does not match the required type. " +
            "Common causes: passing a node where an atomic value is expected (use data($x) or string($x)), " +
            "or a sequence where a single item is required (use exactly-one() or [1]).",
            "https://www.w3.org/TR/xquery-31/#id-typing"),
        ["FORG0001"] = (
            "Invalid value for a cast or constructor. For example, xs:integer('abc') fails. " +
            "Validate the input before casting, or use the castable-as test: `if ($v castable as xs:integer) then xs:integer($v) else 0`.",
            "https://www.w3.org/TR/xpath-functions-31/#ERRFORG0001"),
        ["FORG0006"] = (
            "Invalid argument type for an aggregate function (sum, avg, min, max). " +
            "Aggregates require a sequence of numeric, duration, or other comparable atomic values. " +
            "Use data() to atomize nodes first, or filter the sequence to the right type.",
            "https://www.w3.org/TR/xpath-functions-31/#ERRFORG0006"),
        ["FOAR0001"] = (
            "Division by zero. Guard with `if ($denom ne 0) then $num div $denom else ...` or use idiv (which raises FOAR0001 too — handle the case explicitly).",
            "https://www.w3.org/TR/xpath-functions-31/#ERRFOAR0001"),
        ["FORX0002"] = (
            "Invalid regular expression in fn:matches, fn:replace, fn:tokenize, or fn:analyze-string. " +
            "XQuery regex is a subset of XML Schema regex — no lookahead/lookbehind, no \\Q..\\E. " +
            "Common issue: unescaped { } characters — escape as \\{ \\}.",
            "https://www.w3.org/TR/xpath-functions-31/#regex-syntax"),
        ["FOJS0001"] = (
            "Invalid JSON syntax in fn:parse-json. JSON requires double-quoted strings (not single), " +
            "no trailing commas, and no comments. Use fn:parse-json($input, map{'liberal': true()}) for a more permissive parser.",
            "https://www.w3.org/TR/xpath-functions-31/#func-parse-json"),
        ["FODC0002"] = (
            "fn:doc() failed to retrieve a document. Check the URI is well-formed and reachable. " +
            "For in-memory test data, use xquery-mcp's input arg instead of a real URI.",
            "https://www.w3.org/TR/xpath-functions-31/#ERRFODC0002")
    };
}

using System.ComponentModel;
using System.Text;
using ModelContextProtocol.Server;

namespace XQueryMcpServer;

[McpServerToolType]
public static class LookupTools
{
    [McpServerTool(Name = "xquery_lookup_expression"), Description(
        "Look up an XQuery expression type (e.g., 'FLWOR', 'path', 'typeswitch'). " +
        "Returns syntax, semantics, and examples.")]
    public static string LookupExpression(
        SpecIndex index,
        [Description("Expression name to look up (e.g., 'flwor', 'path', 'if-then-else')")] string name)
    {
        // Search in expressions category first, then fall back to general lookup
        var entries = index.LookupByCategory("expression");
        var entry = entries.FirstOrDefault(e =>
            e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? index.Lookup(name);

        if (entry == null)
        {
            var available = string.Join(", ", entries.Select(e => e.Name));
            return $"Expression '{name}' not found. Available expressions: {available}";
        }

        return FormatEntry(entry);
    }

    [McpServerTool(Name = "xquery_lookup_function"), Description(
        "Look up an XPath/XQuery function by name (e.g., 'fn:tokenize', 'map:merge'). " +
        "Returns signature, parameters, return type, and examples.")]
    public static string LookupFunction(
        SpecIndex index,
        [Description("Function name (e.g., 'fn:tokenize', 'tokenize', 'map:merge')")] string name)
    {
        var entries = index.LookupByCategory("function");
        var entry = entries.FirstOrDefault(e =>
            e.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ||
            e.Name.Equals(name.Replace(':', '-'), StringComparison.OrdinalIgnoreCase)) ?? index.Lookup(name);

        if (entry == null)
        {
            // Try partial match
            var matches = entries.Where(e =>
                e.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (matches.Count > 0)
                return $"Function '{name}' not found. Did you mean: {string.Join(", ", matches.Select(e => e.Name))}";

            return $"Function '{name}' not found. Use xquery_list_functions to see all available functions.";
        }

        return FormatEntry(entry);
    }

    [McpServerTool(Name = "xquery_lookup_prolog"), Description(
        "Look up an XQuery prolog declaration (e.g., 'namespace-declaration', 'function-declaration'). " +
        "Returns syntax and examples.")]
    public static string LookupProlog(
        SpecIndex index,
        [Description("Prolog declaration name (e.g., 'namespace-declaration', 'function-declaration')")] string name)
    {
        var entries = index.LookupByCategory("prolog");
        var entry = entries.FirstOrDefault(e =>
            e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? index.Lookup(name);

        if (entry == null)
        {
            var available = string.Join(", ", entries.Select(e => e.Name));
            return $"Prolog declaration '{name}' not found. Available declarations: {available}";
        }

        return FormatEntry(entry);
    }

    [McpServerTool(Name = "xquery_lookup_error_code"), Description(
        "Look up an XQuery/XPath error code (e.g., 'XPST0003', 'XPDY0002'). " +
        "Returns description, conditions, and fix suggestions.")]
    public static string LookupErrorCode(
        SpecIndex index,
        [Description("Error code (e.g., 'XPST0003', 'FORG0001')")] string code)
    {
        var entries = index.LookupByCategory("error-code");
        var entry = entries.FirstOrDefault(e =>
            e.Name.Equals(code, StringComparison.OrdinalIgnoreCase)) ?? index.Lookup(code);

        if (entry == null)
        {
            var available = string.Join(", ", entries.Select(e => e.Name));
            return $"Error code '{code}' not found. Available error codes: {available}";
        }

        return FormatEntry(entry);
    }

    [McpServerTool(Name = "xquery_search"), Description(
        "Search across all XQuery spec files by keyword. Returns matching entries with relevant excerpts.")]
    public static string Search(
        SpecIndex index,
        [Description("Search query — keywords or phrase")] string query,
        [Description("Maximum results to return (default 10)")] int limit = 10)
    {
        var results = index.Search(query, limit);

        if (results.Count == 0)
            return $"No results found for '{query}'.";

        var sb = new StringBuilder();
        sb.AppendLine($"Found {results.Count} result(s) for '{query}':");
        sb.AppendLine();

        foreach (var (entry, excerpt) in results)
        {
            sb.AppendLine($"## {entry.Name} ({entry.Category})");
            if (entry.SpecUrl != null)
                sb.AppendLine($"Spec: {entry.SpecUrl}");
            sb.AppendLine(excerpt);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "xquery_list_expressions"), Description(
        "List all XQuery expression types with one-line descriptions.")]
    public static string ListExpressions(SpecIndex index)
    {
        var entries = index.LookupByCategory("expression");
        if (entries.Count == 0)
            return "No expression entries found.";

        var sb = new StringBuilder();
        sb.AppendLine("# XQuery Expression Types");
        sb.AppendLine();

        foreach (var entry in entries.OrderBy(e => e.Name))
        {
            sb.AppendLine($"- **{entry.Name}** — {entry.Summary}");
        }

        return sb.ToString();
    }

    [McpServerTool(Name = "xquery_list_functions"), Description(
        "List all XPath/XQuery functions organized by category.")]
    public static string ListFunctions(SpecIndex index)
    {
        var entries = index.LookupByCategory("function");
        if (entries.Count == 0)
            return "No function entries found.";

        // Group by prefix (fn, map, array, etc.) based on file path or name
        var groups = entries
            .GroupBy(e =>
            {
                var name = e.Name;
                var dashIndex = name.IndexOf('-');
                return dashIndex > 0 ? name[..dashIndex] : "other";
            })
            .OrderBy(g => g.Key);

        var sb = new StringBuilder();
        sb.AppendLine("# XPath/XQuery Functions");
        sb.AppendLine();

        foreach (var group in groups)
        {
            var prefix = group.Key switch
            {
                "fn" => "fn (Core Functions)",
                "map" => "map (Map Functions)",
                "array" => "array (Array Functions)",
                _ => group.Key
            };

            sb.AppendLine($"## {prefix}");
            foreach (var entry in group.OrderBy(e => e.Name))
            {
                sb.AppendLine($"- **{entry.Name.Replace('-', ':')}** — {entry.Summary}");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static string FormatEntry(SpecEntry entry)
    {
        var sb = new StringBuilder();
        sb.AppendLine(entry.Content.Trim());

        if (entry.Since != null)
        {
            sb.AppendLine();
            sb.AppendLine($"_Since XQuery {entry.Since}_");
        }

        if (entry.SpecUrl != null)
        {
            sb.AppendLine();
            sb.AppendLine($"Spec: {entry.SpecUrl}");
        }

        return sb.ToString();
    }
}

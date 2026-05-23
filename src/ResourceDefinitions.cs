using System.ComponentModel;
using System.Linq;
using System.Text;
using ModelContextProtocol.Server;

namespace XQueryMcpServer;

[McpServerResourceType]
public static class ResourceDefinitions
{
    [McpServerResource(UriTemplate = "xquery://entries/{name}"), Description(
        "Read a single XQuery spec entry as Markdown.")]
    public static string ReadEntry(SpecIndex index, string name)
    {
        var entry = index.Lookup(name);
        return entry?.Content ?? $"# Not found\n\nNo entry for `{name}` in the XQuery spec corpus.";
    }

    [McpServerResource(UriTemplate = "xquery://index"), Description(
        "List every spec entry as a Markdown index, organized by category.")]
    public static string ReadIndex(SpecIndex index)
    {
        var sb = new StringBuilder("# XQuery Spec Corpus Index\n\n");
        var groups = index.All
            .GroupBy(e => e.Category ?? "uncategorized")
            .OrderBy(g => g.Key);
        foreach (var group in groups)
        {
            sb.AppendLine($"## {group.Key}");
            sb.AppendLine();
            foreach (var entry in group.OrderBy(e => e.Name))
                sb.AppendLine($"- [{entry.Name}](xquery://entries/{entry.Name})");
            sb.AppendLine();
        }
        return sb.ToString();
    }
}

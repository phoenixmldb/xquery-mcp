using System.Text;
using System.Text.RegularExpressions;

namespace XQueryMcpServer;

/// <summary>
/// A single spec entry parsed from a Markdown file with YAML frontmatter.
/// </summary>
public sealed class SpecEntry
{
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required string Content { get; init; }
    public required string FilePath { get; init; }
    public string? Since { get; init; }
    public string? SpecUrl { get; init; }

    /// <summary>
    /// Returns a one-line description extracted from the first paragraph after the heading.
    /// </summary>
    public string Summary
    {
        get
        {
            // Find first non-heading, non-empty line after frontmatter
            var lines = Content.Split('\n');
            var pastHeading = false;
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith('#'))
                {
                    pastHeading = true;
                    continue;
                }
                if (pastHeading && !string.IsNullOrWhiteSpace(trimmed))
                    return trimmed.Length > 120 ? trimmed[..117] + "..." : trimmed;
            }
            return Name;
        }
    }
}

/// <summary>
/// Loads and indexes all XQuery spec Markdown files for searching and lookup.
/// </summary>
public sealed class SpecIndex
{
    private static readonly Regex FrontmatterPattern = new(
        @"^---\s*\n(.*?)\n---\s*\n",
        RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex YamlKeyValue = new(
        @"^(\w[\w_-]*):\s*""?([^""\n]+)""?\s*$",
        RegexOptions.Multiline | RegexOptions.Compiled);

    private readonly Dictionary<string, SpecEntry> _byName = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<SpecEntry>> _byCategory = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<SpecEntry> _all = [];

    public IReadOnlyList<SpecEntry> All => _all;

    public IReadOnlyDictionary<string, List<SpecEntry>> ByCategory => _byCategory;

    /// <summary>
    /// Loads all .md files from the spec directory and its subdirectories.
    /// </summary>
    public static SpecIndex Load(string specPath)
    {
        var index = new SpecIndex();
        var fullPath = Path.GetFullPath(specPath);

        if (!Directory.Exists(fullPath))
            throw new DirectoryNotFoundException($"Spec directory not found: {fullPath}");

        foreach (var file in Directory.EnumerateFiles(fullPath, "*.md", SearchOption.AllDirectories))
        {
            // Skip README files
            if (Path.GetFileName(file).Equals("README.md", StringComparison.OrdinalIgnoreCase))
                continue;

            try
            {
                var content = File.ReadAllText(file, Encoding.UTF8);
                var entry = Parse(content, file);
                if (entry != null)
                    index.Add(entry);
            }
            catch
            {
                // Skip files that fail to parse
            }
        }

        return index;
    }

    private static SpecEntry? Parse(string content, string filePath)
    {
        var match = FrontmatterPattern.Match(content);
        if (!match.Success)
            return null;

        var yaml = match.Groups[1].Value;
        var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (Match kvMatch in YamlKeyValue.Matches(yaml))
        {
            properties[kvMatch.Groups[1].Value] = kvMatch.Groups[2].Value.Trim();
        }

        if (!properties.TryGetValue("name", out var name) ||
            !properties.TryGetValue("category", out var category))
            return null;

        // Body is everything after frontmatter
        var body = content[match.Length..];

        return new SpecEntry
        {
            Name = name,
            Category = category,
            Content = body,
            FilePath = filePath,
            Since = properties.GetValueOrDefault("since"),
            SpecUrl = properties.GetValueOrDefault("spec_url"),
        };
    }

    private void Add(SpecEntry entry)
    {
        _all.Add(entry);

        // Index by name — use the raw name and common aliases
        _byName[entry.Name] = entry;

        // For functions, also index without prefix (e.g., "tokenize" for "fn-tokenize")
        if (entry.Category == "function" && entry.Name.Contains('-'))
        {
            var parts = entry.Name.Split('-', 2);
            if (parts.Length == 2)
            {
                // "fn-tokenize" → also index as "fn:tokenize" and "tokenize"
                _byName[$"{parts[0]}:{parts[1]}"] = entry;
                _byName[parts[1]] = entry;
            }
        }

        // Index by category
        if (!_byCategory.TryGetValue(entry.Category, out var list))
        {
            list = [];
            _byCategory[entry.Category] = list;
        }
        list.Add(entry);
    }

    /// <summary>
    /// Look up a spec entry by exact name match.
    /// </summary>
    public SpecEntry? Lookup(string name)
    {
        // Try exact match first
        if (_byName.TryGetValue(name, out var entry))
            return entry;

        // Try normalized forms: "fn:tokenize" → "fn-tokenize"
        var normalized = name.Replace(':', '-');
        if (_byName.TryGetValue(normalized, out entry))
            return entry;

        // Try case-insensitive substring match on all entries
        return _all.FirstOrDefault(e =>
            e.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Look up entries by category.
    /// </summary>
    public IReadOnlyList<SpecEntry> LookupByCategory(string category)
    {
        return _byCategory.TryGetValue(category, out var list)
            ? list
            : [];
    }

    /// <summary>
    /// Search all entries by keyword across name and content.
    /// </summary>
    public IReadOnlyList<(SpecEntry Entry, string Excerpt)> Search(string query, int maxResults = 10)
    {
        var terms = query.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (terms.Length == 0)
            return [];

        var results = new List<(SpecEntry Entry, string Excerpt, double Score)>();

        foreach (var entry in _all)
        {
            var nameLower = entry.Name.ToLowerInvariant();
            var contentLower = entry.Content.ToLowerInvariant();
            var fullText = nameLower + " " + contentLower;

            // All terms must appear somewhere
            if (!terms.All(t => fullText.Contains(t, StringComparison.Ordinal)))
                continue;

            // Score: name matches are worth more
            var score = 0.0;
            foreach (var term in terms)
            {
                if (nameLower.Contains(term, StringComparison.Ordinal))
                    score += 2.0;
                if (contentLower.Contains(term, StringComparison.Ordinal))
                    score += 1.0;
            }

            // Exact name match bonus
            if (nameLower == query.ToLowerInvariant())
                score += 5.0;

            var excerpt = ExtractExcerpt(entry.Content, terms);
            results.Add((entry, excerpt, score));
        }

        return results
            .OrderByDescending(r => r.Score)
            .Take(maxResults)
            .Select(r => (r.Entry, r.Excerpt))
            .ToList();
    }

    /// <summary>
    /// Loads spec entries from embedded assembly resources.
    /// </summary>
    public static SpecIndex LoadFromAssembly(System.Reflection.Assembly assembly)
    {
        var index = new SpecIndex();

        foreach (var resourceName in assembly.GetManifestResourceNames())
        {
            if (!resourceName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                continue;

            using var stream = assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            var entry = Parse(content, resourceName);
            if (entry != null)
                index.Add(entry);
        }

        return index;
    }

    private static string ExtractExcerpt(string content, string[] terms, int contextChars = 200)
    {
        var contentLower = content.ToLowerInvariant();
        var bestIndex = -1;
        var bestTerm = terms[0];

        // Find the first occurrence of any search term
        foreach (var term in terms)
        {
            var idx = contentLower.IndexOf(term, StringComparison.Ordinal);
            if (idx >= 0 && (bestIndex < 0 || idx < bestIndex))
            {
                bestIndex = idx;
                bestTerm = term;
            }
        }

        if (bestIndex < 0)
            return content.Length > contextChars ? content[..contextChars] + "..." : content;

        var start = Math.Max(0, bestIndex - contextChars / 2);
        var end = Math.Min(content.Length, bestIndex + bestTerm.Length + contextChars / 2);
        var excerpt = content[start..end].Trim();

        if (start > 0) excerpt = "..." + excerpt;
        if (end < content.Length) excerpt += "...";

        return excerpt;
    }
}

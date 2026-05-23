namespace XQueryMcpServer.Models;

public sealed record QueryError(
    string Code,
    string Message,
    int? Line = null,
    int? Column = null,
    string? SourceSnippet = null,
    string? SpecUrl = null);

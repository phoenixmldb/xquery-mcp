namespace XQueryMcpServer.Models;

public sealed record QueryResult(
    bool Ok,
    string? Value,
    int? Count,
    long? ElapsedMs,
    IReadOnlyList<QueryError>? Errors)
{
    public static QueryResult Success(string? value, int? count, long? elapsedMs) =>
        new(true, value, count, elapsedMs, null);

    public static QueryResult Failure(IReadOnlyList<QueryError> errors) =>
        new(false, null, null, null, errors);
}

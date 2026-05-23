using XQueryMcpServer.Models;
using Xunit;

namespace XQueryMcpServer.Tests.Models;

public class QueryResultTests
{
    [Fact]
    public void Success_SerializesWithCount()
    {
        var r = QueryResult.Success("<a/>", count: 1, elapsedMs: 3);
        Assert.True(r.Ok);
        Assert.Equal("<a/>", r.Value);
        Assert.Equal(1, r.Count);
        Assert.Null(r.Errors);
    }

    [Fact]
    public void Failure_CarriesErrorList()
    {
        var err = new QueryError("XPST0003", "syntax error", 1, 5, "1 + ", "https://w3.org/...");
        var r = QueryResult.Failure(new[] { err });
        Assert.False(r.Ok);
        Assert.Single(r.Errors!);
        Assert.Equal("XPST0003", r.Errors![0].Code);
    }
}

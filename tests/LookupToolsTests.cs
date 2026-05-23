using System.Text.Json;
using XQueryMcpServer;
using Xunit;

namespace XQueryMcpServer.Tests;

public class LookupToolsTests
{
    private static SpecIndex GetIndex() =>
        SpecIndex.LoadFromAssembly(typeof(SpecIndex).Assembly);

    [Fact]
    public void CompareVersions_FnTokenize_ReturnsSince_3_1()
    {
        var json = LookupTools.CompareVersions(GetIndex(), "fn:tokenize");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("fn-tokenize", doc.RootElement.GetProperty("name").GetString());
        Assert.Equal("3.1", doc.RootElement.GetProperty("since").GetString());
        Assert.Equal("function", doc.RootElement.GetProperty("category").GetString());
    }

    [Fact]
    public void CompareVersions_HyphenatedNameWithoutPrefix_FallsBackToFn()
    {
        // Regression: hyphenated names like "parse-json" must fall back to "fn:parse-json".
        // Earlier guard `!name.Contains('-')` blocked this — XQuery function names are
        // routinely hyphenated, only ':' indicates a namespace.
        var json = LookupTools.CompareVersions(GetIndex(), "parse-json");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
    }

    [Fact]
    public void CompareVersions_UnknownName_ReturnsOkFalse()
    {
        var json = LookupTools.CompareVersions(GetIndex(), "fn:does-not-exist");
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("fn:does-not-exist", doc.RootElement.GetProperty("name").GetString());
        Assert.True(doc.RootElement.GetProperty("message").GetString()!.Length > 0);
    }

    [Fact]
    public void FindExamples_Flwor_ReturnsExample()
    {
        var json = LookupTools.FindExamples(GetIndex(), "flwor");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.True(doc.RootElement.GetProperty("count").GetInt32() >= 1);
    }

    [Fact]
    public void FindExamples_NoMatch_ReturnsEmpty()
    {
        var json = LookupTools.FindExamples(GetIndex(), "totally-fake-feature-xyz");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal(0, doc.RootElement.GetProperty("count").GetInt32());
    }
}

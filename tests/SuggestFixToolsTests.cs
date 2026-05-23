using System.Text.Json;
using XQueryMcpServer;
using Xunit;

namespace XQueryMcpServer.Tests;

public class SuggestFixToolsTests
{
    [Fact]
    public void Suggest_XPST0003_OffersSyntaxHint()
    {
        var json = SuggestFixTools.Suggest("XPST0003");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Contains("syntax", doc.RootElement.GetProperty("suggestion").GetString(),
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Suggest_UnknownCode_ReturnsOkFalse()
    {
        var json = SuggestFixTools.Suggest("XPXX9999");
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("ok").GetBoolean());
    }

    [Fact]
    public void Suggest_LowerCaseCode_NormalizesAndMatches()
    {
        var json = SuggestFixTools.Suggest("xpst0003");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("XPST0003", doc.RootElement.GetProperty("code").GetString());
    }
}

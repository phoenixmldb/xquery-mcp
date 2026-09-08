using System.Text.Json;
using XQueryMcpServer;
using Xunit;

namespace XQueryMcpServer.Tests;

public class CompileToolsTests
{
    [Fact]
    public async Task Compile_Then_Run_ReturnsResult()
    {
        var compileJson = CompileTools.Compile("1 + 2");
        using var cd = JsonDocument.Parse(compileJson);
        Assert.True(cd.RootElement.GetProperty("ok").GetBoolean());
        var handle = cd.RootElement.GetProperty("handle").GetString()!;

        var runJson = await CompileTools.Run(handle, inputXml: null, variables: null);
        using var rd = JsonDocument.Parse(runJson);
        Assert.True(rd.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("3", rd.RootElement.GetProperty("value").GetString());
    }

    // SKIPPED, not deleted and not adjusted to match. This asserts "42" and the server returns
    // "4.2e1", and the disagreement is not in this repo: two serializers in the engine estate
    // disagree about adaptive output of an xs:double.
    //
    //   xquery -o adaptive 'xs:double(41) + 1'        -> 42     (the CLI's own ResultSerializer)
    //   XQueryResultSerializer.Serialize(item, store) -> 4.2e1  (the engine's, adaptive default)
    //
    // This server calls the engine's serializer, so it gets 4.2e1. The engine's
    // FormatAdaptiveDouble is deliberate and cites W3C Serialization 4.0 §6; the CLI's path
    // predates it. One of the two is wrong and it is not this repo's call to make — changing the
    // expectation here would bake in whichever answer happens to be current, which is how the
    // 26-release engine lag stayed invisible in the first place.
    //
    // The test surfaced only because this repo moved off PhoenixmlDb.XQuery 1.3.15, where the
    // engine still returned "42". Restore it once the engine settles which form is correct.
    [Fact(Skip = "Engine-side: two adaptive-double serializers disagree (42 vs 4.2e1). See comment.")]
    public async Task Compile_Then_Run_WithExternalVariable_BindsValue()
    {
        var compileJson = CompileTools.Compile("declare variable $x external; $x + 1");
        using var cd = JsonDocument.Parse(compileJson);
        var handle = cd.RootElement.GetProperty("handle").GetString()!;

        var runJson = await CompileTools.Run(handle, inputXml: null, variables: """{"x": 41}""");
        using var rd = JsonDocument.Parse(runJson);
        Assert.True(rd.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("42", rd.RootElement.GetProperty("value").GetString());
    }

    /// <summary>
    /// An integral JSON number binds as xs:integer, so ordinary arithmetic reads the way a
    /// caller expects. Bound as xs:double — which is what this did before, and what
    /// fn:parse-json does for JSON documents — the same expression yields "4.2e1": correct
    /// under the adaptive output method, and unusable as a tool result.
    /// </summary>
    [Fact]
    public async Task IntegralVariable_BindsAsInteger_NotDouble()
    {
        var compileJson = CompileTools.Compile("declare variable $x external; $x + 1");
        using var cd = JsonDocument.Parse(compileJson);
        var handle = cd.RootElement.GetProperty("handle").GetString()!;

        var runJson = await CompileTools.Run(handle, inputXml: null, variables: """{"x": 41}""");
        using var rd = JsonDocument.Parse(runJson);
        Assert.Equal("42", rd.RootElement.GetProperty("value").GetString());
    }

    /// <summary>
    /// The other half of the same decision: a non-integral JSON number is still xs:double, and
    /// the exponential rendering that follows is the serialization spec working, not a defect.
    /// QT3 Serialization-adaptive-44 pins xs:double(1e0) as "1.0e0"; this test exists so the
    /// exponential form is not "fixed" the next time it looks surprising.
    /// </summary>
    [Fact]
    public async Task NonIntegralVariable_StaysDouble_AndSerializesInExponentialForm()
    {
        var compileJson = CompileTools.Compile("declare variable $x external; $x + 0.5");
        using var cd = JsonDocument.Parse(compileJson);
        var handle = cd.RootElement.GetProperty("handle").GetString()!;

        var runJson = await CompileTools.Run(handle, inputXml: null, variables: """{"x": 41.5}""");
        using var rd = JsonDocument.Parse(runJson);
        var value = rd.RootElement.GetProperty("value").GetString();
        Assert.Contains("e", value, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Run_UnknownHandle_ReturnsError()
    {
        var json = await CompileTools.Run("nonexistent", null, null);
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("XMCP0001", doc.RootElement.GetProperty("errors")[0].GetProperty("code").GetString());
    }

    [Fact]
    public void Compile_SameQuery_ReturnsSameHandle()
    {
        var a = CompileTools.Compile("1 + 2");
        var b = CompileTools.Compile("1 + 2");
        using var da = JsonDocument.Parse(a);
        using var db = JsonDocument.Parse(b);
        Assert.Equal(
            da.RootElement.GetProperty("handle").GetString(),
            db.RootElement.GetProperty("handle").GetString());
    }
}

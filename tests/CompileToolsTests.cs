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

    [Fact]
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

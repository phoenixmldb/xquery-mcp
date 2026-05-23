using System.Text.Json;
using XQueryMcpServer;
using Xunit;

namespace XQueryMcpServer.Tests;

public class ExecutionToolsTests
{
    [Fact]
    public async Task Evaluate_SyntaxError_ReturnsStructuredJson()
    {
        var json = await ExecutionTools.Evaluate("1 + ", null);
        using var doc = JsonDocument.Parse(json);
        var ok = doc.RootElement.GetProperty("ok").GetBoolean();
        Assert.False(ok);
        var errs = doc.RootElement.GetProperty("errors");
        Assert.True(errs.GetArrayLength() >= 1);
        var first = errs[0];
        Assert.StartsWith("XPST", first.GetProperty("code").GetString());
    }

    [Fact]
    public async Task Evaluate_Success_ReturnsStructuredValueWithType()
    {
        var json = await ExecutionTools.Evaluate("1 + 2", null);
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("3", doc.RootElement.GetProperty("value").GetString());
    }

    [Fact]
    public async Task Evaluate_RuntimeError_ReturnsStructuredJson()
    {
        var json = await ExecutionTools.Evaluate("1 div 0", null);
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.True(doc.RootElement.GetProperty("errors").GetArrayLength() >= 1);
    }

    [Fact]
    public async Task Evaluate_EmptySequence_SetsCountZero()
    {
        var json = await ExecutionTools.Evaluate("()", null);
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal(0, doc.RootElement.GetProperty("count").GetInt32());
        Assert.False(doc.RootElement.TryGetProperty("value", out _));
    }

    [Fact]
    public void Validate_BadQuery_ReturnsStructuredErrors()
    {
        var json = ExecutionTools.Validate("for $x at $i in (1,2)"); // missing return
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.True(doc.RootElement.GetProperty("errors").GetArrayLength() >= 1);
    }

    [Fact]
    public void Validate_GoodQuery_ReturnsOkTrue()
    {
        var json = ExecutionTools.Validate("1 + 2");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
    }

    [Fact]
    public async Task EvaluateXPath_Success_ReturnsStructured()
    {
        var json = await ExecutionTools.EvaluateXPath("count(/r/i)", "<r><i/><i/></r>");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("2", doc.RootElement.GetProperty("value").GetString());
    }

    [Fact]
    public async Task EvaluateXPath_SyntaxError_ReturnsStructuredErrors()
    {
        var json = await ExecutionTools.EvaluateXPath("count(", "<r/>");
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.True(doc.RootElement.GetProperty("errors").GetArrayLength() >= 1);
    }

    [Fact]
    public async Task Evaluate_WithExternalVariable_BindsValue()
    {
        var json = await ExecutionTools.Evaluate(
            "declare variable $name external; concat('hello ', $name)",
            inputXml: null,
            variables: """{"name": "world"}""");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("hello world", doc.RootElement.GetProperty("value").GetString());
    }

    [Fact]
    public async Task Evaluate_BadVariablesJson_ReturnsStructuredError()
    {
        var json = await ExecutionTools.Evaluate("1 + 1", inputXml: null, variables: "not-json");
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.Equal("XMCP0002", doc.RootElement.GetProperty("errors")[0].GetProperty("code").GetString());
    }

    [Fact]
    public async Task Test_Pass_AtomicResult()
    {
        var json = await ExecutionTools.Test("1 + 2", null, "3");
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("passed").GetBoolean());
    }

    [Fact]
    public async Task Test_Fail_AtomicResult()
    {
        var json = await ExecutionTools.Test("1 + 2", null, "4");
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("passed").GetBoolean());
    }

    [Fact]
    public async Task Test_QueryError_ReturnsPassedFalseWithError()
    {
        var json = await ExecutionTools.Test("1 + ", null, "anything");
        using var doc = JsonDocument.Parse(json);
        Assert.False(doc.RootElement.GetProperty("passed").GetBoolean());
        Assert.True(doc.RootElement.TryGetProperty("error", out _));
    }
}

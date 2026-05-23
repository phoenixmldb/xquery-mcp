using System.Reflection;
using System.Text.Json;
using ModelContextProtocol.Server;
using XQueryMcpServer;
using Xunit;

namespace XQueryMcpServer.Tests;

public class CapabilityToolsTests
{
    private static SpecIndex GetIndex() =>
        SpecIndex.LoadFromAssembly(typeof(SpecIndex).Assembly);

    [Fact]
    public void Capabilities_ReportsEngineVersionAndSpecCounts()
    {
        var json = CapabilityTools.Get(GetIndex());
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.GetProperty("ok").GetBoolean());
        Assert.NotNull(doc.RootElement.GetProperty("engineVersion").GetString());
        Assert.True(doc.RootElement.GetProperty("specEntryCount").GetInt32() > 0);
        Assert.True(doc.RootElement.GetProperty("tools").GetArrayLength() > 5);
    }

    [Fact]
    public void Capabilities_ToolList_MatchesRegisteredTools()
    {
        var registered = typeof(CapabilityTools).Assembly.GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
            .Select(m => m.GetCustomAttribute<McpServerToolAttribute>())
            .Where(a => a != null)
            .Select(a => a!.Name)
            .OrderBy(n => n)
            .ToArray();

        var json = CapabilityTools.Get(GetIndex());
        using var doc = JsonDocument.Parse(json);
        var listed = doc.RootElement.GetProperty("tools").EnumerateArray()
            .Select(e => e.GetString()!)
            .OrderBy(n => n)
            .ToArray();

        Assert.Equal(registered, listed);
    }
}

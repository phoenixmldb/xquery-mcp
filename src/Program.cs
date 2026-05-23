using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using XQueryMcpServer;

// Resolve spec data: env var > CLI arg > filesystem fallback > embedded resources
var specPath = Environment.GetEnvironmentVariable("XQUERY_SPEC_PATH");

for (var i = 0; i < args.Length; i++)
{
    if (args[i] is "--spec-path" && i + 1 < args.Length)
        specPath = args[++i];
}

SpecIndex index;
if (specPath != null)
{
    specPath = Path.GetFullPath(specPath);
    index = SpecIndex.Load(specPath);
    await Console.Error.WriteLineAsync(
        $"[xquery-mcp] Loaded {index.All.Count} spec entries from {specPath}");
}
else
{
    // Two levels up from bin/Debug/net10.0/ = server/, three = xquery-spec-mcp/
    var fsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    if (Directory.Exists(fsPath) &&
        Directory.EnumerateFiles(fsPath, "*.md", SearchOption.AllDirectories)
            .Any(f => !f.Contains("server", StringComparison.OrdinalIgnoreCase)))
    {
        index = SpecIndex.Load(fsPath);
        await Console.Error.WriteLineAsync(
            $"[xquery-mcp] Loaded {index.All.Count} spec entries from {fsPath}");
    }
    else
    {
        index = SpecIndex.LoadFromAssembly(typeof(SpecIndex).Assembly);
        await Console.Error.WriteLineAsync(
            $"[xquery-mcp] Loaded {index.All.Count} spec entries from embedded resources");
    }
}

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(options =>
{
    // MCP uses stdio — log to stderr only
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddSingleton(index);
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly()
    .WithPromptsFromAssembly()
    .WithResourcesFromAssembly();

var host = builder.Build();
await host.RunAsync();

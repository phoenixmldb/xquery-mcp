using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace XQueryMcpServer;

[McpServerToolType]
public static class CapabilityTools
{
    [McpServerTool(Name = "server_capabilities"), Description(
        "Report what this MCP server can do: engine type and version, spec coverage stats, and supported feature flags. " +
        "Call this once at the start of a session to know which features (update facility, schema-aware, maps/arrays, etc.) you can rely on.")]
    public static string Get(SpecIndex index)
    {
        var engineAsm = typeof(PhoenixmlDb.XQuery.XQueryFacade).Assembly;
        return JsonSerializer.Serialize(new
        {
            ok = true,
            server = "xquery-mcp",
            serverVersion = typeof(CapabilityTools).Assembly.GetName().Version?.ToString(),
            engine = "PhoenixmlDb.XQuery",
            engineVersion = engineAsm.GetName().Version?.ToString(),
            specs = new[] { "XQuery 3.1", "XQuery 4.0 (draft)", "XPath 3.1", "XPath 4.0 (draft)", "XQuery Update Facility 3.0" },
            specEntryCount = index.All.Count,
            features = new
            {
                updateFacility = true,
                xpath40 = true,
                xquery40Draft = true,
                schemaAware = false,
                fnTransform = true,
                jsonFunctions = true,
                mapsAndArrays = true
            },
            tools = new[]
            {
                "xquery_evaluate", "xquery_validate", "xquery_compile", "xquery_run",
                "xquery_test", "xquery_lookup_expression", "xquery_lookup_function",
                "xquery_lookup_prolog", "xquery_lookup_error_code", "xquery_search",
                "xquery_list_expressions", "xquery_list_functions",
                "xquery_compare_versions", "xquery_find_examples", "xquery_suggest_fix",
                "xquery_explain_error", "xpath_evaluate", "xml_validate_schema", "xml_format",
                "server_capabilities"
            }
        }, XQueryErrorMapper.JsonOpts);
    }
}

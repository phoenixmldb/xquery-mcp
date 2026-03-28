# xquery-mcp

An MCP server for XQuery -- spec reference lookup, query validation, and XQuery/XPath execution powered by the [PhoenixmlDb](https://github.com/phoenixmldb) engine.

## Install

### Option 1: Self-contained binary (no .NET required)

Download the latest release for your platform from [GitHub Releases](https://github.com/phoenixmldb/xquery-mcp/releases):

| Platform | Download |
|----------|----------|
| Linux x64 | `xquery-mcp-linux-x64` |
| Linux ARM64 | `xquery-mcp-linux-arm64` |
| macOS x64 | `xquery-mcp-osx-x64` |
| macOS ARM64 | `xquery-mcp-osx-arm64` |
| Windows x64 | `xquery-mcp-win-x64.exe` |

```bash
# Linux/macOS
chmod +x xquery-mcp-linux-x64
./xquery-mcp-linux-x64
```

### Option 2: .NET tool (requires .NET 10 SDK)

```bash
dotnet tool install -g xquery-mcp
```

## Configure

Add to your MCP client configuration:

**Claude Code** (`.mcp.json`):
```json
{
  "mcpServers": {
    "xquery": {
      "command": "xquery-mcp"
    }
  }
}
```

**Claude Desktop** (`claude_desktop_config.json`):
```json
{
  "mcpServers": {
    "xquery": {
      "command": "xquery-mcp",
      "args": []
    }
  }
}
```

## Tools

### Spec Reference
| Tool | Description |
|------|-------------|
| `xquery_lookup_expression` | Look up XQuery expressions -- FLWOR, path, typeswitch, etc. |
| `xquery_lookup_function` | Look up XPath/XQuery functions -- signature, parameters, examples |
| `xquery_lookup_prolog` | Look up prolog declarations -- namespaces, functions, modules |
| `xquery_lookup_error_code` | Look up error codes with descriptions and fix suggestions |
| `xquery_search` | Full-text search across all spec entries |
| `xquery_list_expressions` | List all expression types |
| `xquery_list_functions` | List all functions by category |

### Execution
| Tool | Description |
|------|-------------|
| `xquery_evaluate` | Execute an XQuery expression with optional XML input |
| `xquery_validate` | Compile a query without executing -- check for errors |
| `xpath_evaluate` | Evaluate an XPath expression against XML |
| `xquery_explain_error` | Explain an error code with causes and fixes |

### Utilities
| Tool | Description |
|------|-------------|
| `xml_validate_schema` | Validate XML against an XSD schema |
| `xml_format` | Pretty-print XML |

## License

Apache-2.0

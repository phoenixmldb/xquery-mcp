# xquery-mcp

An MCP server for XQuery — spec reference lookup, query validation, and XQuery/XPath execution powered by the [PhoenixmlDb](https://github.com/phoenixmldb) engine.

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
| `xquery_lookup_expression` | Look up XQuery expressions — FLWOR, path, typeswitch, etc. |
| `xquery_lookup_function` | Look up XPath/XQuery functions — signature, parameters, examples |
| `xquery_lookup_prolog` | Look up prolog declarations — namespaces, functions, modules |
| `xquery_lookup_error_code` | Look up error codes with descriptions and fix suggestions |
| `xquery_search` | Full-text search across all spec entries |
| `xquery_list_expressions` | List all expression types |
| `xquery_list_functions` | List all functions by category |

### Execution
| Tool | Description |
|------|-------------|
| `xquery_evaluate` | Execute an XQuery expression — returns structured JSON with errors, value, and timing |
| `xquery_validate` | Compile a query without executing — check for errors |
| `xquery_compile` | Compile a query to a reusable SHA256 handle |
| `xquery_run` | Execute a compiled handle against input with optional external variable bindings |
| `xquery_test` | Assert a query result equals an expected value (red/green TDD) |
| `xpath_evaluate` | Evaluate an XPath expression against XML |
| `xquery_explain_error` | Explain an error code with causes and fixes |

### Spec-Aware Analysis
| Tool | Description |
|------|-------------|
| `xquery_compare_versions` | Report which spec version introduced a function or expression |
| `xquery_find_examples` | Find curated, working XQuery examples by topic |
| `xquery_suggest_fix` | Given an error code, return a spec-grounded fix suggestion |
| `server_capabilities` | Report engine version, spec coverage, and supported features |

### Utilities
| Tool | Description |
|------|-------------|
| `xml_validate_schema` | Validate XML against an XSD schema |
| `xml_format` | Pretty-print XML |

## How It Works

The MCP server is built from two things: **spec reference files** and the **PhoenixmlDb XQuery engine**.

### Spec Files

The `spec/` directory contains Markdown files with YAML frontmatter covering XQuery 3.1, XPath 3.1, Functions & Operators, and the Update Facility. Each file documents one expression type, function, prolog declaration, error code, or concept:

```
spec/
  expressions/      # 20 files — FLWOR, path, typeswitch, quantified, ...
  functions/         # 70+ files organized by category
    array/           # array:size, array:get, array:join, ...
    boolean/         # fn:boolean, fn:true, fn:false, fn:not
    json/            # fn:json-doc, fn:parse-json, fn:json-to-xml, ...
    map/             # map:merge, map:get, map:keys, ...
    numeric/         # fn:count, fn:sum, fn:avg, fn:round, ...
    sequence/        # fn:head, fn:tail, fn:filter, fn:sort, ...
    string/          # fn:tokenize, fn:replace, fn:matches, ...
  prolog/            # 10 files — namespace, function, variable declarations
  update-facility/   # 6 files — insert, delete, replace, rename, transform
  error-codes/       # 23 files — XPST0003, XPTY0004, FORG0001, ...
  concepts/          # 6 files — atomization, focus, serialization, ...
```

Every file has this structure:

```markdown
---
name: FLWOR
category: expression
since: "1.0"
spec_url: https://www.w3.org/TR/xquery-31/#id-flwor-expressions
---

# FLWOR Expressions

The primary iteration and binding mechanism in XQuery.

## Syntax
...
```

At startup, the server loads all Markdown files, parses the YAML frontmatter, and builds an in-memory index. The lookup tools search this index by name, category, or full-text keywords. The content is returned directly to the AI agent as Markdown.

### Execution Engine

The execution tools (`xquery_evaluate`, `xquery_validate`, `xpath_evaluate`) delegate to the [PhoenixmlDb.XQuery](https://www.nuget.org/packages/PhoenixmlDb.XQuery) engine — a .NET implementation of XQuery 3.1 with Full-Text Search and Update Facility. This means the MCP server can not only look up how a feature *should* work, but also run it and show you the actual output.

### Embedded Resources

When installed as a dotnet tool or self-contained binary, the spec files are embedded as assembly resources — no external files needed. For local development, you can override the spec path:

```bash
XQUERY_SPEC_PATH=/path/to/spec xquery-mcp
# or
xquery-mcp --spec-path /path/to/spec
```

## Spec Coverage and Contributing

The spec reference files were AI-generated from the W3C XQuery 3.1, XPath 3.1, and Functions & Operators specifications. They are interpretive summaries derived from the published standards — useful as reference for AI agents, but not a verbatim reproduction of the spec text. Every entry includes a `spec_url` linking to the authoritative W3C source.

**We're actively filling gaps and improving accuracy.** The initial release was generated from an LLM's training data without systematically checking against the spec's table of contents, which means coverage skewed toward frequently-discussed features and missed advanced/niche areas.

### Areas where we'd welcome help

- **Full-Text Search** — The XQuery Full-Text extension (`ft:contains`, `fts:` functions, match options) is lightly covered.
- **Update Facility** — Transform expressions, pending update lists, and compatibility rules need deeper coverage.
- **Window clauses** — Tumbling and sliding window syntax in FLWOR expressions.
- **Group by / order by** — Detailed behavior for edge cases (empty sequences, collations, stable ordering).
- **Error code completeness** — The XQuery/XPath specs define 100+ error codes. We have 23.
- **Serialization** — The full serialization parameter matrix for XQuery output.
- **Schema-aware types** — Schema imports, type annotations, and validate expressions.

### How to contribute

1. Fork this repo
2. Add or edit files in `spec/` — follow the existing YAML frontmatter + Markdown format
3. Submit a PR

The only requirements are: the `name`, `category`, and `since` frontmatter fields must be present, and the content should be accurate to the W3C spec. If you're unsure about a detail, include a note and link to the relevant spec section — an incomplete-but-honest entry is better than a confident-but-wrong one.

## License

Apache-2.0

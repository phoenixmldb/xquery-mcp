# xquery-mcp release notes

## 1.4.0 — 2026-05-23

Major release combining four shippable phases of work. Consumers upgrading from 1.0.1 will see all of these features at once.

### Structured execution returns (was Phase A)
- All execution tools now return JSON `QueryResult` envelopes instead of plain strings.
- Shape: `{ ok, value?, count?, elapsedMs?, errors? }` with errors carrying `{ code, message, line, column, sourceSnippet }`.
- Empty XQuery sequences signal as `count: 0` with no `value` key — agents can disambiguate empty sequence from empty string.
- Runtime exception handling narrowed to `XQueryRuntimeException` and `XQueryException` — no more silently swallowed OOM/cancellation.
- Affected tools: `xquery_evaluate`, `xquery_validate`, `xpath_evaluate`.

### Compile handles + external variables (was Phase B)
- New `xquery_compile` returns a SHA256-keyed handle for a compiled query.
- New `xquery_run` applies a handle to input + optional external variables, skipping re-compilation.
- `xquery_evaluate` and `xquery_run` accept a JSON `variables` arg to bind XQuery external variable declarations: strings as `xs:string`, numbers as `xs:double`, booleans as `xs:boolean`.
- Error codes: `XMCP0001` for unknown handle, `XMCP0002` for invalid variables JSON.

### Spec-aware tooling (was Phase C)
- `xquery_compare_versions` — for any function or expression, returns when the spec introduced it. Hyphenated function names without prefix fall back to `fn:` namespace correctly (e.g., `parse-json` resolves to `fn:parse-json`).
- `xquery_find_examples` — returns curated working examples by topic. Ships with 8 hand-authored examples: FLWOR, try-catch, window-clause, map-constructor, array-flatten, update-facility, fn:transform, module-import.
- `xquery_suggest_fix` — given an error code, returns a spec-grounded actionable suggestion. Top-10 rules cover XPST0003, XPST0008, XPST0017, XPTY0004, FORG0001, FORG0006, FOAR0001, FORX0002, FOJS0001, FODC0002.
- `xquery_test` — assertion runner. Apply a query to input and compare the result to an expected value; returns pass/fail with a diff. Wraps the actual result in a synthetic root so DeepEquals works on sequence-typed results.

### MCP discoverability (was Phase D)
- `server_capabilities` — reports engine type and version, spec coverage stats, feature flags, and the complete tool list. Call once at session start to know what you can rely on.
- 4 MCP prompts: `xquery-write-flwor`, `xquery-migrate-1-to-3`, `xquery-debug-query`, `xquery-write-test` — surfaced as slash commands in MCP clients.
- MCP resources: browse the spec corpus via `xquery://index` and `xquery://entries/{name}` URIs without explicit tool calls.

### Notes / known limitations
- `xquery_compile`'s handle cache is process-lifetime with no eviction. Acceptable for the single-user MCP server model.
- `QueryResult.Type` field was removed in late-cycle review — schema now matches what tools actually populate.

## 1.0.1 — earlier

Bumps PhoenixmlDb.XQuery 1.1.0.5 → 1.3.15. Picks up 20+ versions of engine fixes including the JSON serializer conformance work (QT3 method-json 64/74 → 73/74), full source-location coverage, Phase 2.5 perf (2.6× win), Martin Honnen bug fixes, and library-module decimal-format scoping. No MCP API changes.

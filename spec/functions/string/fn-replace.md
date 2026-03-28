---
name: fn-replace
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-replace
---

# fn:replace

Replaces parts of a string matching a regular expression pattern.

## Signature

`fn:replace($input as xs:string?, $pattern as xs:string, $replacement as xs:string) as xs:string`
`fn:replace($input as xs:string?, $pattern as xs:string, $replacement as xs:string, $flags as xs:string) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$input` | `xs:string?` | The input string |
| `$pattern` | `xs:string` | Regular expression pattern |
| `$replacement` | `xs:string` | Replacement string. Use `$1`, `$2`, etc. for captured groups; `$$` for literal `$`. |
| `$flags` | `xs:string` | Optional flags: `s` (dot-all), `m` (multi-line), `i` (case-insensitive), `x` (extended), `q` (literal) |

## Semantics

- Replaces every substring matching `$pattern` with `$replacement`.
- The replacement string can reference captured groups: `$0` for the whole match, `$1`..`$9` for groups.
- If `$input` is the empty sequence, returns the zero-length string.
- The pattern must not match a zero-length string.

## Examples

```xquery
fn:replace("Hello World", "World", "XQuery")
(: Result: "Hello XQuery" :)

fn:replace("2024-01-15", "(\d{4})-(\d{2})-(\d{2})", "$2/$3/$1")
(: Result: "01/15/2024" :)

fn:replace("aAbBcC", "[a-c]", "X", "i")
(: Result: "XXXXXX" :)

fn:replace("hello   world", "\s+", " ")
(: Result: "hello world" :)
```

## Error Codes

- `FORX0002` — Invalid regular expression
- `FORX0003` — Pattern matches a zero-length string
- `FORX0004` — Invalid replacement string (e.g., `$` not followed by a digit)

## See Also

- [fn-matches](fn-matches.md)
- [fn-tokenize](fn-tokenize.md)

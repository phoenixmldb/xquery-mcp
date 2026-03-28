---
name: fn-contains
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-contains
---

# fn:contains

Tests whether a string contains a given substring.

## Signature

`fn:contains($arg1 as xs:string?, $arg2 as xs:string?) as xs:boolean`
`fn:contains($arg1 as xs:string?, $arg2 as xs:string?, $collation as xs:string) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg1` | `xs:string?` | The string to search in |
| `$arg2` | `xs:string?` | The substring to search for |
| `$collation` | `xs:string` | Optional collation URI |

## Semantics

- Returns `true` if `$arg1` contains `$arg2` as a substring.
- If `$arg2` is the zero-length string or empty sequence, returns `true`.
- If `$arg1` is the empty sequence, it is treated as the zero-length string.

## Examples

```xquery
fn:contains("Hello World", "World")
(: Result: true :)

fn:contains("Hello", "hello")
(: Result: false — case-sensitive :)

fn:contains("Hello", "")
(: Result: true :)

fn:contains((), "test")
(: Result: false :)
```

## Error Codes

- `FOCH0002` — Unsupported collation

## See Also

- [fn-starts-with](fn-starts-with.md)
- [fn-ends-with](fn-ends-with.md)
- [fn-matches](fn-matches.md)

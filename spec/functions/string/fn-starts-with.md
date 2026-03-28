---
name: fn-starts-with
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-starts-with
---

# fn:starts-with

Tests whether a string starts with a given prefix.

## Signature

`fn:starts-with($arg1 as xs:string?, $arg2 as xs:string?) as xs:boolean`
`fn:starts-with($arg1 as xs:string?, $arg2 as xs:string?, $collation as xs:string) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg1` | `xs:string?` | The string to test |
| `$arg2` | `xs:string?` | The prefix to check for |
| `$collation` | `xs:string` | Optional collation URI |

## Semantics

- Returns `true` if `$arg1` starts with `$arg2`.
- If `$arg2` is the zero-length string or empty sequence, returns `true`.
- If `$arg1` is the empty sequence, it is treated as the zero-length string.

## Examples

```xquery
fn:starts-with("Hello World", "Hello")
(: Result: true :)

fn:starts-with("Hello", "hello")
(: Result: false :)

fn:starts-with("Hello", "")
(: Result: true :)
```

## Error Codes

- `FOCH0002` — Unsupported collation

## See Also

- [fn-ends-with](fn-ends-with.md)
- [fn-contains](fn-contains.md)

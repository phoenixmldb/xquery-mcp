---
name: fn-ends-with
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-ends-with
---

# fn:ends-with

Tests whether a string ends with a given suffix.

## Signature

`fn:ends-with($arg1 as xs:string?, $arg2 as xs:string?) as xs:boolean`
`fn:ends-with($arg1 as xs:string?, $arg2 as xs:string?, $collation as xs:string) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg1` | `xs:string?` | The string to test |
| `$arg2` | `xs:string?` | The suffix to check for |
| `$collation` | `xs:string` | Optional collation URI |

## Semantics

- Returns `true` if `$arg1` ends with `$arg2`.
- If `$arg2` is the zero-length string or empty sequence, returns `true`.
- If `$arg1` is the empty sequence, it is treated as the zero-length string.

## Examples

```xquery
fn:ends-with("Hello World", "World")
(: Result: true :)

fn:ends-with("Hello", "HELLO")
(: Result: false :)

fn:ends-with("Hello", "")
(: Result: true :)
```

## Error Codes

- `FOCH0002` — Unsupported collation

## See Also

- [fn-starts-with](fn-starts-with.md)
- [fn-contains](fn-contains.md)

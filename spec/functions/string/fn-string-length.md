---
name: fn-string-length
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-string-length
---

# fn:string-length

Returns the number of characters in a string.

## Signature

`fn:string-length() as xs:integer`
`fn:string-length($arg as xs:string?) as xs:integer`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:string?` | The string to measure. If omitted, uses the string value of the context item. |

## Semantics

- Returns the number of characters (Unicode code points) in the string.
- If `$arg` is the empty sequence, returns 0.
- If called with no argument, uses `fn:string(.)` (the string value of the context item).

## Examples

```xquery
fn:string-length("Hello")
(: Result: 5 :)

fn:string-length("")
(: Result: 0 :)

fn:string-length(())
(: Result: 0 :)

fn:string-length("caf&#233;")
(: Result: 4 :)
```

## Error Codes

- `XPDY0002` — Context item is absent when called with no argument

## See Also

- [fn-substring](fn-substring.md)
- [fn-string](fn-string.md)

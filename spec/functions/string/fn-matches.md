---
name: fn-matches
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-matches
---

# fn:matches

Tests whether a string matches a regular expression pattern.

## Signature

`fn:matches($input as xs:string?, $pattern as xs:string) as xs:boolean`
`fn:matches($input as xs:string?, $pattern as xs:string, $flags as xs:string) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$input` | `xs:string?` | The string to test |
| `$pattern` | `xs:string` | Regular expression pattern |
| `$flags` | `xs:string` | Optional flags: `s` (dot-all), `m` (multi-line), `i` (case-insensitive), `x` (extended), `q` (literal) |

## Semantics

- Returns `true` if `$input` matches `$pattern` (the match can be anywhere in the string; use `^` and `$` anchors for full-string match).
- If `$input` is the empty sequence, it is treated as the zero-length string.

## Examples

```xquery
fn:matches("Hello", "ell")
(: Result: true :)

fn:matches("Hello", "^H.*o$")
(: Result: true :)

fn:matches("Hello", "hello", "i")
(: Result: true :)

fn:matches("12345", "^\d+$")
(: Result: true :)

fn:matches((), "test")
(: Result: false :)
```

## Error Codes

- `FORX0002` — Invalid regular expression

## See Also

- [fn-replace](fn-replace.md)
- [fn-tokenize](fn-tokenize.md)
- [fn-contains](fn-contains.md)

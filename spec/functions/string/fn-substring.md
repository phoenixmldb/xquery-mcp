---
name: fn-substring
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-substring
---

# fn:substring

Returns a portion of a string.

## Signature

`fn:substring($sourceString as xs:string?, $start as xs:double) as xs:string`
`fn:substring($sourceString as xs:string?, $start as xs:double, $length as xs:double) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$sourceString` | `xs:string?` | The source string |
| `$start` | `xs:double` | Starting position (1-based), rounded to nearest integer |
| `$length` | `xs:double` | Number of characters to extract, rounded to nearest integer |

## Semantics

- Positions are 1-based.
- `$start` and `$length` are rounded using `fn:round` (round half to even is NOT used; standard round-half-up applies).
- If `$start` is less than 1, the substring starts from position 1 but the length is reduced accordingly.
- If `$sourceString` is the empty sequence, returns the zero-length string.
- The returned substring contains characters at positions `P` where `$start <= P < $start + $length`.

## Examples

```xquery
fn:substring("Hello", 2)
(: Result: "ello" :)

fn:substring("Hello", 2, 3)
(: Result: "ell" :)

fn:substring("Hello", 1, 3)
(: Result: "Hel" :)

fn:substring("Hello", 0, 3)
(: Result: "He" — position 0 to 2.x, so chars at 1 and 2 :)

fn:substring((), 1, 3)
(: Result: "" :)

fn:substring("12345", 1.5, 2.6)
(: Result: "234" — round(1.5)=2, round(2.6)=3, positions 2..4 :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-string-length](fn-string-length.md)
- [fn-contains](fn-contains.md)

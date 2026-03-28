---
name: fn-tokenize
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-tokenize
---

# fn:tokenize

Splits a string into a sequence of substrings using a regular expression pattern as the delimiter.

## Signature

`fn:tokenize($input as xs:string?) as xs:string*`
`fn:tokenize($input as xs:string?, $pattern as xs:string) as xs:string*`
`fn:tokenize($input as xs:string?, $pattern as xs:string, $flags as xs:string) as xs:string*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$input` | `xs:string?` | The string to split |
| `$pattern` | `xs:string` | Regular expression delimiter pattern |
| `$flags` | `xs:string` | Optional regex flags |

## Semantics

- With one argument, splits on whitespace (after stripping leading/trailing whitespace). Equivalent to `fn:tokenize(fn:normalize-space($input), ' ')`.
- With two or three arguments, splits `$input` at each occurrence of `$pattern`.
- The pattern must not match a zero-length string.
- If `$input` is the empty sequence, returns the empty sequence.

## Examples

```xquery
fn:tokenize("  hello  world  ")
(: Result: ("hello", "world") :)

fn:tokenize("a,b,,c", ",")
(: Result: ("a", "b", "", "c") :)

fn:tokenize("one:two:three", ":")
(: Result: ("one", "two", "three") :)

fn:tokenize("red, green, blue", ",\s*")
(: Result: ("red", "green", "blue") :)
```

## Error Codes

- `FORX0002` — Invalid regular expression
- `FORX0003` — Pattern matches a zero-length string

## See Also

- [fn-string-join](fn-string-join.md)
- [fn-matches](fn-matches.md)
- [fn-replace](fn-replace.md)

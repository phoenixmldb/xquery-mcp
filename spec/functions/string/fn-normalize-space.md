---
name: fn-normalize-space
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-normalize-space
---

# fn:normalize-space

Strips leading and trailing whitespace and collapses internal whitespace sequences to single spaces.

## Signature

`fn:normalize-space() as xs:string`
`fn:normalize-space($arg as xs:string?) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:string?` | The string to normalize. If omitted, uses the string value of the context item. |

## Semantics

- Strips leading and trailing whitespace characters (spaces, tabs, newlines, carriage returns).
- Replaces all internal sequences of whitespace characters with a single space.
- If `$arg` is the empty sequence, returns the zero-length string.

## Examples

```xquery
fn:normalize-space("  hello   world  ")
(: Result: "hello world" :)

fn:normalize-space("line1&#10;line2&#10;line3")
(: Result: "line1 line2 line3" :)

fn:normalize-space(())
(: Result: "" :)
```

## Error Codes

- `XPDY0002` — Context item is absent when called with no argument

## See Also

- [fn-string](fn-string.md)
- [fn-tokenize](fn-tokenize.md)

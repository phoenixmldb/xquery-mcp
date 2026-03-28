---
name: fn-upper-case
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-upper-case
---

# fn:upper-case

Converts a string to upper case.

## Signature

`fn:upper-case($arg as xs:string?) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:string?` | The string to convert |

## Semantics

- Converts each character in the string to its upper-case equivalent using Unicode case mappings.
- Characters with no upper-case equivalent are unchanged.
- If `$arg` is the empty sequence, returns the zero-length string.

## Examples

```xquery
fn:upper-case("hello")
(: Result: "HELLO" :)

fn:upper-case("Hello World")
(: Result: "HELLO WORLD" :)

fn:upper-case(())
(: Result: "" :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-lower-case](fn-lower-case.md)

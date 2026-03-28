---
name: fn-lower-case
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-lower-case
---

# fn:lower-case

Converts a string to lower case.

## Signature

`fn:lower-case($arg as xs:string?) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:string?` | The string to convert |

## Semantics

- Converts each character in the string to its lower-case equivalent using Unicode case mappings.
- Characters with no lower-case equivalent are unchanged.
- If `$arg` is the empty sequence, returns the zero-length string.

## Examples

```xquery
fn:lower-case("HELLO")
(: Result: "hello" :)

fn:lower-case("Hello World")
(: Result: "hello world" :)

fn:lower-case(())
(: Result: "" :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-upper-case](fn-upper-case.md)

---
name: fn-concat
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-concat
---

# fn:concat

Concatenates two or more string values.

## Signature

`fn:concat($arg1 as xs:anyAtomicType?, $arg2 as xs:anyAtomicType?, ...) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg1` | `xs:anyAtomicType?` | First value to concatenate |
| `$arg2` | `xs:anyAtomicType?` | Second value to concatenate |
| `...` | `xs:anyAtomicType?` | Additional values (variadic — at least 2 arguments required) |

## Semantics

- Accepts two or more arguments (this is the only XPath function with a variable number of arguments).
- Each argument is atomized and cast to `xs:string`. Empty sequences are treated as zero-length strings.
- Returns the concatenation of all string values.

Note: For concatenating sequences of strings with a separator, use `fn:string-join` instead. The XQuery string concatenation operator `||` is equivalent to `fn:concat` with two arguments.

## Examples

```xquery
fn:concat("Hello", " ", "World")
(: Result: "Hello World" :)

fn:concat("Value: ", 42)
(: Result: "Value: 42" :)

fn:concat("a", "b", "c", "d")
(: Result: "abcd" :)

fn:concat("Price: $", (), "0.00")
(: Result: "Price: $0.00" :)

(: Equivalent to || operator for two args :)
"Hello" || " " || "World"
```

## Error Codes

- `XPTY0004` — An argument cannot be atomized or cast to string
- `FORG0001` — Invalid cast to string

## See Also

- [fn-string-join](fn-string-join.md)
- [fn-string](fn-string.md)

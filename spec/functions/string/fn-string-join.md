---
name: fn-string-join
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-string-join
---

# fn:string-join

Joins a sequence of strings with an optional separator.

## Signature

`fn:string-join($arg1 as xs:anyAtomicType*) as xs:string`
`fn:string-join($arg1 as xs:anyAtomicType*, $arg2 as xs:string) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg1` | `xs:anyAtomicType*` | Sequence of values to join |
| `$arg2` | `xs:string` | Separator string (defaults to zero-length string) |

## Semantics

- Each item in `$arg1` is cast to `xs:string`.
- The strings are concatenated with `$arg2` inserted between consecutive items.
- If `$arg1` is empty, returns the zero-length string.
- If `$arg1` has one item, returns that item as a string (no separator inserted).

## Examples

```xquery
fn:string-join(("a", "b", "c"), ", ")
(: Result: "a, b, c" :)

fn:string-join(("Hello", "World"))
(: Result: "HelloWorld" :)

fn:string-join(1 to 5, "-")
(: Result: "1-2-3-4-5" :)

fn:string-join((), ", ")
(: Result: "" :)

fn:string-join(//name, "; ")
```

## Error Codes

- `XPTY0004` — An item in `$arg1` cannot be cast to string
- `FORG0001` — Invalid cast to string

## See Also

- [fn-concat](fn-concat.md)
- [fn-tokenize](fn-tokenize.md)

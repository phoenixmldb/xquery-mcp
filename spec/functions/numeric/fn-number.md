---
name: fn-number
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-number
---

# fn:number

Converts the argument to `xs:double`.

## Signature

`fn:number() as xs:double`
`fn:number($arg as xs:anyAtomicType?) as xs:double`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:anyAtomicType?` | Value to convert. If omitted, uses the context item. |

## Semantics

- If `$arg` is the empty sequence, returns `NaN`.
- If `$arg` can be cast to `xs:double`, returns the result.
- If `$arg` cannot be cast to `xs:double`, returns `NaN` (no error is raised).
- If called with no argument, the context item is atomized and then converted.

## Examples

```xquery
fn:number("42")
(: Result: 42.0e0 :)

fn:number("3.14")
(: Result: 3.14e0 :)

fn:number("not-a-number")
(: Result: NaN :)

fn:number(())
(: Result: NaN :)

fn:number(true())
(: Result: 1.0e0 :)
```

## Error Codes

- `XPDY0002` — Context item is absent when called with no argument

## See Also

- [fn-sum](fn-sum.md)
- [fn-round](fn-round.md)

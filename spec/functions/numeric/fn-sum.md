---
name: fn-sum
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-sum
---

# fn:sum

Returns the sum of a sequence of numeric values.

## Signature

`fn:sum($arg as xs:anyAtomicType*) as xs:anyAtomicType`
`fn:sum($arg as xs:anyAtomicType*, $zero as xs:anyAtomicType?) as xs:anyAtomicType?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:anyAtomicType*` | Sequence of values to sum |
| `$zero` | `xs:anyAtomicType?` | Value to return if `$arg` is empty (default: `0`) |

## Semantics

- Values are atomized and then summed using the `+` operator.
- `xs:untypedAtomic` values are cast to `xs:double`.
- If `$arg` is empty, returns `$zero` (default: integer `0`).
- If any value is `NaN`, the result is `NaN`.
- Duration values can be summed if they are all `xs:dayTimeDuration` or all `xs:yearMonthDuration`.

## Examples

```xquery
fn:sum((1, 2, 3, 4, 5))
(: Result: 15 :)

fn:sum(())
(: Result: 0 :)

fn:sum((), ())
(: Result: () :)

fn:sum(//price)

fn:sum((1.5, 2.5, 3.0))
(: Result: 7.0 :)
```

## Error Codes

- `FORG0006` — Invalid argument type (e.g., mixing incompatible types)

## See Also

- [fn-count](fn-count.md)
- [fn-avg](fn-avg.md)

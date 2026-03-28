---
name: fn-avg
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-avg
---

# fn:avg

Returns the average of a sequence of numeric values.

## Signature

`fn:avg($arg as xs:anyAtomicType*) as xs:anyAtomicType?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:anyAtomicType*` | Sequence of values to average |

## Semantics

- Values are atomized. `xs:untypedAtomic` values are cast to `xs:double`.
- If `$arg` is empty, returns the empty sequence.
- Computes `fn:sum($arg) div fn:count($arg)`.
- If any value is `NaN`, the result is `NaN`.

## Examples

```xquery
fn:avg((1, 2, 3, 4, 5))
(: Result: 3.0 :)

fn:avg(())
(: Result: () :)

fn:avg((10.0, 20.0))
(: Result: 15.0 :)

fn:avg(//employee/salary)
```

## Error Codes

- `FORG0006` — Invalid argument type

## See Also

- [fn-sum](fn-sum.md)
- [fn-count](fn-count.md)
- [fn-min](fn-min.md)
- [fn-max](fn-max.md)

---
name: fn-max
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-max
---

# fn:max

Returns the maximum value from a sequence.

## Signature

`fn:max($arg as xs:anyAtomicType*) as xs:anyAtomicType?`
`fn:max($arg as xs:anyAtomicType*, $collation as xs:string) as xs:anyAtomicType?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:anyAtomicType*` | Sequence of values |
| `$collation` | `xs:string` | Optional collation for string comparison |

## Semantics

- Values are atomized. `xs:untypedAtomic` values are cast to `xs:double`.
- If `$arg` is empty, returns the empty sequence.
- Compares values using the `gt` operator.
- If any value is `NaN`, the result is `NaN`.

## Examples

```xquery
fn:max((3, 1, 4, 1, 5))
(: Result: 5 :)

fn:max(("a", "b", "c"))
(: Result: "c" :)

fn:max(())
(: Result: () :)

fn:max(//product/price)
```

## Error Codes

- `FORG0006` — Incompatible types for comparison

## See Also

- [fn-min](fn-min.md)
- [fn-avg](fn-avg.md)

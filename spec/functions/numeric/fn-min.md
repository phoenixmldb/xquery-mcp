---
name: fn-min
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-min
---

# fn:min

Returns the minimum value from a sequence.

## Signature

`fn:min($arg as xs:anyAtomicType*) as xs:anyAtomicType?`
`fn:min($arg as xs:anyAtomicType*, $collation as xs:string) as xs:anyAtomicType?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:anyAtomicType*` | Sequence of values |
| `$collation` | `xs:string` | Optional collation for string comparison |

## Semantics

- Values are atomized. `xs:untypedAtomic` values are cast to `xs:double`.
- If `$arg` is empty, returns the empty sequence.
- Compares values using the `lt` operator.
- If any value is `NaN`, the result is `NaN`.

## Examples

```xquery
fn:min((3, 1, 4, 1, 5))
(: Result: 1 :)

fn:min(("a", "b", "c"))
(: Result: "a" :)

fn:min(())
(: Result: () :)

fn:min(//product/price)
```

## Error Codes

- `FORG0006` — Incompatible types for comparison

## See Also

- [fn-max](fn-max.md)
- [fn-avg](fn-avg.md)

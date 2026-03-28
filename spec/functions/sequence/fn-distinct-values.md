---
name: fn-distinct-values
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-distinct-values
---

# fn:distinct-values

Returns a sequence with duplicate values removed.

## Signature

`fn:distinct-values($arg as xs:anyAtomicType*) as xs:anyAtomicType*`
`fn:distinct-values($arg as xs:anyAtomicType*, $collation as xs:string) as xs:anyAtomicType*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:anyAtomicType*` | The sequence of atomic values |
| `$collation` | `xs:string` | Optional collation for string comparison |

## Semantics

- Returns a sequence containing the unique values from `$arg`.
- Values are compared using the `eq` operator.
- `NaN` is considered equal to `NaN`.
- The order of returned values is implementation-dependent.
- `xs:untypedAtomic` values are compared as `xs:untypedAtomic` (not cast to string or double).

## Examples

```xquery
fn:distinct-values((1, 2, 2, 3, 3, 3))
(: Result: (1, 2, 3) — order may vary :)

fn:distinct-values(("a", "b", "a", "c"))
(: Result: ("a", "b", "c") — order may vary :)

fn:distinct-values(//employee/department)

fn:distinct-values((1, 1.0))
(: Result: (1) — numeric type promotion means these are equal :)
```

## Error Codes

- `FOCH0002` — Unsupported collation

## See Also

- [fn-sort](fn-sort.md)
- [fn-count](../numeric/fn-count.md)

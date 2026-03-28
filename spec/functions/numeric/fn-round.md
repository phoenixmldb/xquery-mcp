---
name: fn-round
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-round
---

# fn:round

Rounds a number to the nearest integer or to a specified number of decimal places.

## Signature

`fn:round($arg as xs:numeric?) as xs:numeric?`
`fn:round($arg as xs:numeric?, $precision as xs:integer) as xs:numeric?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:numeric?` | The number to round |
| `$precision` | `xs:integer` | Number of decimal places (default: 0). Negative values round to powers of 10. |

## Semantics

- Rounds to the nearest value at the specified precision.
- When the value is exactly halfway, rounds towards positive infinity (round-half-up for positive numbers).
- If `$arg` is the empty sequence, returns the empty sequence.
- Result type matches input type.
- `round(NaN)` returns `NaN`. `round(INF)` returns `INF`.

## Examples

```xquery
fn:round(2.5)
(: Result: 3.0 :)

fn:round(-2.5)
(: Result: -2.0 :)

fn:round(1.125, 2)
(: Result: 1.13 :)

fn:round(1234, -2)
(: Result: 1200 :)

fn:round(())
(: Result: () :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-floor](fn-floor.md)
- [fn-ceiling](fn-ceiling.md)
- [fn-abs](fn-abs.md)
- [fn-format-number](fn-format-number.md)

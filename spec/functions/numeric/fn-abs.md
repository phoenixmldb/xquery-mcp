---
name: fn-abs
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-abs
---

# fn:abs

Returns the absolute value of a number.

## Signature

`fn:abs($arg as xs:numeric?) as xs:numeric?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:numeric?` | The numeric value |

## Semantics

- Returns the absolute value of `$arg`.
- If `$arg` is the empty sequence, returns the empty sequence.
- The result type matches the input type (`xs:integer` in, `xs:integer` out, etc.).
- `abs(NaN)` returns `NaN`. `abs(-0.0e0)` returns `+0.0e0`. `abs(-INF)` returns `INF`.

## Examples

```xquery
fn:abs(-5)
(: Result: 5 :)

fn:abs(5)
(: Result: 5 :)

fn:abs(-3.14)
(: Result: 3.14 :)

fn:abs(())
(: Result: () :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-round](fn-round.md)
- [fn-floor](fn-floor.md)
- [fn-ceiling](fn-ceiling.md)

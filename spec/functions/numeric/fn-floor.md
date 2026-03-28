---
name: fn-floor
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-floor
---

# fn:floor

Returns the largest integer less than or equal to the argument (rounds towards negative infinity).

## Signature

`fn:floor($arg as xs:numeric?) as xs:numeric?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:numeric?` | The numeric value |

## Semantics

- Returns the largest (closest to positive infinity) value that is not greater than `$arg` and is an integer.
- If `$arg` is the empty sequence, returns the empty sequence.
- Result type matches input type.

## Examples

```xquery
fn:floor(2.9)
(: Result: 2.0 :)

fn:floor(-2.1)
(: Result: -3.0 :)

fn:floor(5)
(: Result: 5 :)

fn:floor(())
(: Result: () :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-ceiling](fn-ceiling.md)
- [fn-round](fn-round.md)

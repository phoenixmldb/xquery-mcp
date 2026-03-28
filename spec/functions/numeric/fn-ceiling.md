---
name: fn-ceiling
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-ceiling
---

# fn:ceiling

Returns the smallest integer greater than or equal to the argument (rounds towards positive infinity).

## Signature

`fn:ceiling($arg as xs:numeric?) as xs:numeric?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `xs:numeric?` | The numeric value |

## Semantics

- Returns the smallest (closest to negative infinity) value that is not less than `$arg` and is an integer.
- If `$arg` is the empty sequence, returns the empty sequence.
- Result type matches input type.

## Examples

```xquery
fn:ceiling(2.1)
(: Result: 3.0 :)

fn:ceiling(-2.9)
(: Result: -2.0 :)

fn:ceiling(5)
(: Result: 5 :)

fn:ceiling(())
(: Result: () :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-floor](fn-floor.md)
- [fn-round](fn-round.md)

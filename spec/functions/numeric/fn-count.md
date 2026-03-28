---
name: fn-count
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-count
---

# fn:count

Returns the number of items in a sequence.

## Signature

`fn:count($arg as item()*) as xs:integer`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()*` | The sequence to count |

## Semantics

- Returns the number of items in `$arg`.
- If `$arg` is the empty sequence, returns `0`.

## Examples

```xquery
fn:count((1, 2, 3))
(: Result: 3 :)

fn:count(())
(: Result: 0 :)

fn:count(//book)

fn:count("hello")
(: Result: 1 :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-sum](fn-sum.md)
- [fn-empty](../sequence/fn-empty.md)
- [fn-exists](../sequence/fn-exists.md)

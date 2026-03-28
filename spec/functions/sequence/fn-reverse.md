---
name: fn-reverse
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-reverse
---

# fn:reverse

Reverses the order of items in a sequence.

## Signature

`fn:reverse($arg as item()*) as item()*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()*` | The sequence to reverse |

## Semantics

- Returns a new sequence with items in reverse order.
- If `$arg` is empty, returns the empty sequence.

## Examples

```xquery
fn:reverse((1, 2, 3, 4))
(: Result: (4, 3, 2, 1) :)

fn:reverse(("a", "b", "c"))
(: Result: ("c", "b", "a") :)

fn:reverse(())
(: Result: () :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-sort](fn-sort.md)
- [fn-head](fn-head.md)
- [fn-tail](fn-tail.md)

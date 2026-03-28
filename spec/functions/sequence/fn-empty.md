---
name: fn-empty
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-empty
---

# fn:empty

Returns `true` if the argument is the empty sequence.

## Signature

`fn:empty($arg as item()*) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()*` | The sequence to test |

## Semantics

- Returns `true` if `$arg` contains zero items.
- Returns `false` otherwise.
- Equivalent to `count($arg) eq 0`.

## Examples

```xquery
fn:empty(())
(: Result: true :)

fn:empty((1, 2, 3))
(: Result: false :)

fn:empty(//nonexistent)
(: Result: true :)

fn:empty("")
(: Result: false — a zero-length string is still one item :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-exists](fn-exists.md)
- [fn-count](../numeric/fn-count.md)

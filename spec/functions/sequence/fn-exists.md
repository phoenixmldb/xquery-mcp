---
name: fn-exists
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-exists
---

# fn:exists

Returns `true` if the argument is a non-empty sequence.

## Signature

`fn:exists($arg as item()*) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()*` | The sequence to test |

## Semantics

- Returns `true` if `$arg` contains one or more items.
- Returns `false` if `$arg` is the empty sequence.
- Equivalent to `not(empty($arg))`.

## Examples

```xquery
fn:exists((1, 2, 3))
(: Result: true :)

fn:exists(())
(: Result: false :)

fn:exists(//book)
(: Result: true if any book elements exist :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-empty](fn-empty.md)
- [fn-count](../numeric/fn-count.md)

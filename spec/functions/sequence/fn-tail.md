---
name: fn-tail
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-tail
---

# fn:tail

Returns all items in a sequence except the first.

## Signature

`fn:tail($arg as item()*) as item()*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()*` | The input sequence |

## Semantics

- Returns all items in `$arg` except the first.
- If `$arg` is empty or has one item, returns the empty sequence.
- Equivalent to `subsequence($arg, 2)`.

## Examples

```xquery
fn:tail((1, 2, 3))
(: Result: (2, 3) :)

fn:tail(("only"))
(: Result: () :)

fn:tail(())
(: Result: () :)

(: Recursive processing :)
declare function local:sum($seq as xs:integer*) as xs:integer {
  if (fn:empty($seq)) then 0
  else fn:head($seq) + local:sum(fn:tail($seq))
};
```

## Error Codes

None specific to this function.

## See Also

- [fn-head](fn-head.md)
- [fn-reverse](fn-reverse.md)

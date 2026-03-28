---
name: fn-head
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-head
---

# fn:head

Returns the first item in a sequence.

## Signature

`fn:head($arg as item()*) as item()?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()*` | The input sequence |

## Semantics

- Returns the first item in `$arg`.
- If `$arg` is the empty sequence, returns the empty sequence.
- Equivalent to `$arg[1]`.

## Examples

```xquery
fn:head((1, 2, 3))
(: Result: 1 :)

fn:head(())
(: Result: () :)

fn:head("only")
(: Result: "only" :)

fn:head(//book)
(: First book element in document order :)
```

## Error Codes

None specific to this function.

## See Also

- [fn-tail](fn-tail.md)
- [fn-last](fn-last.md)

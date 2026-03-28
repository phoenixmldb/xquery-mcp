---
name: fn-sort
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-sort
---

# fn:sort

Sorts a sequence using optional collation and key functions.

## Signature

`fn:sort($input as item()*) as item()*`
`fn:sort($input as item()*, $collation as xs:string?) as item()*`
`fn:sort($input as item()*, $collation as xs:string?, $key as function(item()) as xs:anyAtomicType*) as item()*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$input` | `item()*` | The sequence to sort |
| `$collation` | `xs:string?` | Collation URI for string comparison (use `()` for default) |
| `$key` | `function(item()) as xs:anyAtomicType*` | Function to compute sort keys |

## Semantics

- Sorts `$input` in ascending order.
- Without a key function, items are atomized and sorted by their values.
- With a key function, each item's sort key is computed by applying the function.
- The sort is stable: items with equal keys retain their original order.

## Examples

```xquery
fn:sort((3, 1, 4, 1, 5))
(: Result: (1, 1, 3, 4, 5) :)

fn:sort(("banana", "apple", "cherry"))
(: Result: ("apple", "banana", "cherry") :)

(: Sort with key function :)
fn:sort(//book, (), function($b) { $b/title/string() })

(: Sort by string length :)
fn:sort(("cc", "a", "bbb"), (), function($s) { string-length($s) })
(: Result: ("a", "cc", "bbb") :)

(: Sort maps :)
let $people := (
  map{"name":"Charlie", "age":30},
  map{"name":"Alice", "age":25},
  map{"name":"Bob", "age":35}
)
return fn:sort($people, (), function($p) { $p?age })
```

## Error Codes

- `XPTY0004` — Type error in comparison

## See Also

- [fn-reverse](fn-reverse.md)
- [fn-distinct-values](fn-distinct-values.md)

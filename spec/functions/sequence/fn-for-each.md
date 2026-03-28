---
name: fn-for-each
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-for-each
---

# fn:for-each

Applies a function to each item in a sequence and returns the concatenated results.

## Signature

`fn:for-each($seq as item()*, $action as function(item()) as item()*) as item()*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$seq` | `item()*` | The input sequence |
| `$action` | `function(item()) as item()*` | Function to apply to each item |

## Semantics

- Applies `$action` to each item in `$seq` in order.
- Concatenates all results into a single sequence.
- Equivalent to `for $item in $seq return $action($item)`.

## Examples

```xquery
fn:for-each(1 to 5, function($x) { $x * $x })
(: Result: (1, 4, 9, 16, 25) :)

fn:for-each(("hello", "world"), upper-case#1)
(: Result: ("HELLO", "WORLD") :)

fn:for-each(//book, function($b) { $b/title/string() })

(: Chained with arrow operator :)
(1 to 5) => for-each(function($x) { $x * 2 })
(: Result: (2, 4, 6, 8, 10) :)
```

## Error Codes

- `XPTY0004` — Type error when applying the function

## See Also

- [fn-filter](fn-filter.md)
- [fn-sort](fn-sort.md)

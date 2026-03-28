---
name: fn-filter
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-filter
---

# fn:filter

Returns items from a sequence for which a predicate function returns true.

## Signature

`fn:filter($seq as item()*, $f as function(item()) as xs:boolean) as item()*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$seq` | `item()*` | The input sequence |
| `$f` | `function(item()) as xs:boolean` | Predicate function |

## Semantics

- Applies `$f` to each item in `$seq`.
- Returns items for which `$f` returns `true`.
- The order of items is preserved.

## Examples

```xquery
fn:filter(1 to 10, function($x) { $x mod 2 = 0 })
(: Result: (2, 4, 6, 8, 10) :)

fn:filter(("apple", "", "banana", ""), function($s) { $s ne "" })
(: Result: ("apple", "banana") :)

fn:filter(//book, function($b) { $b/price < 20 })

(: Chained with arrow operator :)
(1 to 20)
  => filter(function($x) { $x mod 3 = 0 })
  => for-each(function($x) { $x * 10 })
(: Result: (30, 60, 90, 120, 150, 180) :)
```

## Error Codes

- `XPTY0004` — Predicate function does not return `xs:boolean`

## See Also

- [fn-for-each](fn-for-each.md)
- [fn-sort](fn-sort.md)

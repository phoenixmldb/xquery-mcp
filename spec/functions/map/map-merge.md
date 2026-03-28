---
name: map-merge
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-map-merge
---

# map:merge

Merges a sequence of maps into a single map.

## Signature

`map:merge($maps as map(*)*) as map(*)`
`map:merge($maps as map(*)*, $options as map(*)) as map(*)`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$maps` | `map(*)*` | Sequence of maps to merge |
| `$options` | `map(*)` | Options: `duplicates` controls handling of duplicate keys |

## Semantics

- Returns a new map containing all entries from the input maps.
- The `duplicates` option controls behavior when the same key appears in multiple maps:
  - `"use-first"` (default) — use the value from the first map
  - `"use-last"` — use the value from the last map
  - `"combine"` — combine values into a sequence
  - `"reject"` — raise an error
  - `"use-any"` — implementation may use any value

## Examples

```xquery
map:merge((map{"a":1}, map{"b":2}))
(: Result: map{"a":1, "b":2} :)

map:merge((map{"a":1}, map{"a":2}))
(: Result: map{"a":1} — use-first is default :)

map:merge((map{"a":1}, map{"a":2}), map{"duplicates":"use-last"})
(: Result: map{"a":2} :)

map:merge((map{"a":1}, map{"a":2}), map{"duplicates":"combine"})
(: Result: map{"a":(1,2)} :)

(: Merge sequence of maps :)
map:merge(
  for $book in //book
  return map { $book/@isbn/string(): $book/title/string() }
)
```

## Error Codes

- `FOJS0003` — Duplicate key when `duplicates` is `"reject"`

## See Also

- [map-put](map-put.md)
- [map-remove](map-remove.md)

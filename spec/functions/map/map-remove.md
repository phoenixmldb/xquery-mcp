---
name: map-remove
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-map-remove
---

# map:remove

Returns a new map with specified entries removed.

## Signature

`map:remove($map as map(*), $keys as xs:anyAtomicType*) as map(*)`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$map` | `map(*)` | The original map |
| `$keys` | `xs:anyAtomicType*` | Keys to remove |

## Semantics

- Returns a new map that is a copy of `$map` with the entries for `$keys` removed.
- Keys not present in the map are silently ignored.
- The original map is not modified.

## Examples

```xquery
map:remove(map{"a":1, "b":2, "c":3}, "b")
(: Result: map{"a":1, "c":3} :)

map:remove(map{"a":1, "b":2}, ("a", "b"))
(: Result: map{} :)

map:remove(map{"a":1}, "missing")
(: Result: map{"a":1} :)
```

## Error Codes

None specific to this function.

## See Also

- [map-put](map-put.md)
- [map-merge](map-merge.md)

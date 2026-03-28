---
name: map-size
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-map-size
---

# map:size

Returns the number of entries in a map.

## Signature

`map:size($map as map(*)) as xs:integer`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$map` | `map(*)` | The map |

## Semantics

- Returns the number of key-value entries in the map.
- For an empty map, returns 0.

## Examples

```xquery
map:size(map{"a":1, "b":2, "c":3})
(: Result: 3 :)

map:size(map{})
(: Result: 0 :)
```

## Error Codes

None specific to this function.

## See Also

- [map-keys](map-keys.md)
- [map-contains](map-contains.md)

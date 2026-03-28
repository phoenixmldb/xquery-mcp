---
name: map-keys
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-map-keys
---

# map:keys

Returns a sequence containing all keys in a map.

## Signature

`map:keys($map as map(*)) as xs:anyAtomicType*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$map` | `map(*)` | The map |

## Semantics

- Returns a sequence of all keys in the map.
- The order of keys is implementation-dependent.
- If the map is empty, returns the empty sequence.

## Examples

```xquery
map:keys(map{"a":1, "b":2, "c":3})
(: Result: ("a", "b", "c") — order may vary :)

map:keys(map{})
(: Result: () :)

(: Iterate over map entries :)
let $m := map{"x":10, "y":20}
for $key in map:keys($m)
return $key || "=" || $m($key)
```

## Error Codes

None specific to this function.

## See Also

- [map-contains](map-contains.md)
- [map-get](map-get.md)
- [map-size](map-size.md)

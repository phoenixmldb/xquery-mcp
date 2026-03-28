---
name: map-contains
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-map-contains
---

# map:contains

Tests whether a map contains an entry with a given key.

## Signature

`map:contains($map as map(*), $key as xs:anyAtomicType) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$map` | `map(*)` | The map to test |
| `$key` | `xs:anyAtomicType` | The key to look for |

## Semantics

- Returns `true` if the map contains an entry with the specified key.
- Keys are compared using the `eq` operator.

## Examples

```xquery
map:contains(map{"a":1, "b":2}, "a")
(: Result: true :)

map:contains(map{"a":1, "b":2}, "c")
(: Result: false :)

map:contains(map{}, "any")
(: Result: false :)
```

## Error Codes

None specific to this function.

## See Also

- [map-get](map-get.md)
- [map-keys](map-keys.md)

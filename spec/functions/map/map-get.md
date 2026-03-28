---
name: map-get
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-map-get
---

# map:get

Returns the value associated with a given key in a map.

## Signature

`map:get($map as map(*), $key as xs:anyAtomicType) as item()*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$map` | `map(*)` | The map |
| `$key` | `xs:anyAtomicType` | The key to look up |

## Semantics

- Returns the value associated with `$key` in `$map`.
- If the key is not present, returns the empty sequence.
- Equivalent to `$map($key)` or `$map?key` (for string keys with NCName syntax).

## Examples

```xquery
map:get(map{"a":1, "b":2}, "a")
(: Result: 1 :)

map:get(map{"a":1}, "missing")
(: Result: () :)

(: Equivalent forms :)
let $m := map{"name": "Alice"}
return (
  map:get($m, "name"),  (: "Alice" :)
  $m("name"),            (: "Alice" :)
  $m?name               (: "Alice" :)
)
```

## Error Codes

None specific to this function.

## See Also

- [map-contains](map-contains.md)
- [map-put](map-put.md)

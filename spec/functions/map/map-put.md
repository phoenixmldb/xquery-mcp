---
name: map-put
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-map-put
---

# map:put

Returns a new map with an added or updated entry.

## Signature

`map:put($map as map(*), $key as xs:anyAtomicType, $value as item()*) as map(*)`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$map` | `map(*)` | The original map |
| `$key` | `xs:anyAtomicType` | The key for the entry |
| `$value` | `item()*` | The value for the entry |

## Semantics

- Returns a new map that is a copy of `$map` with the entry for `$key` set to `$value`.
- If `$key` already exists, its value is replaced.
- If `$key` does not exist, a new entry is added.
- The original map is not modified (maps are immutable).

## Examples

```xquery
map:put(map{"a":1}, "b", 2)
(: Result: map{"a":1, "b":2} :)

map:put(map{"a":1}, "a", 99)
(: Result: map{"a":99} :)

(: Building a map incrementally :)
let $m := map{}
let $m := map:put($m, "x", 10)
let $m := map:put($m, "y", 20)
return $m
(: Result: map{"x":10, "y":20} :)
```

## Error Codes

None specific to this function.

## See Also

- [map-get](map-get.md)
- [map-remove](map-remove.md)
- [map-merge](map-merge.md)

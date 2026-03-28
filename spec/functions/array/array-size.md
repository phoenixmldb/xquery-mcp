---
name: array-size
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-array-size
---

# array:size

Returns the number of members in an array.

## Signature

`array:size($array as array(*)) as xs:integer`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$array` | `array(*)` | The array |

## Semantics

- Returns the number of members in the array.
- An empty array has size 0.

## Examples

```xquery
array:size([1, 2, 3])
(: Result: 3 :)

array:size([])
(: Result: 0 :)

array:size(["a", ["b", "c"]])
(: Result: 2 :)
```

## Error Codes

None specific to this function.

## See Also

- [array-get](array-get.md)
- [array-flatten](array-flatten.md)

---
name: array-get
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-array-get
---

# array:get

Returns the member at a given position in an array.

## Signature

`array:get($array as array(*), $position as xs:integer) as item()*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$array` | `array(*)` | The array |
| `$position` | `xs:integer` | The 1-based position |

## Semantics

- Returns the member at position `$position` (1-based).
- Equivalent to `$array($position)`.
- Raises an error if the position is out of bounds.

## Examples

```xquery
array:get(["a", "b", "c"], 2)
(: Result: "b" :)

array:get([10, 20, 30], 1)
(: Result: 10 :)

(: Equivalent to calling the array as a function :)
let $arr := [10, 20, 30]
return $arr(2)
(: Result: 20 :)
```

## Error Codes

- `FOAY0001` — Position is less than 1 or greater than `array:size($array)`

## See Also

- [array-size](array-size.md)
- [array-flatten](array-flatten.md)

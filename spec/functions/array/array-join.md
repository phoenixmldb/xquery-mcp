---
name: array-join
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-array-join
---

# array:join

Concatenates the members of a sequence of arrays into a single array.

## Signature

`array:join($arrays as array(*)*) as array(*)`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arrays` | `array(*)*` | Sequence of arrays to concatenate |

## Semantics

- Returns a new array whose members are the members of all input arrays, in order.
- If the input is an empty sequence, returns an empty array.

## Examples

```xquery
array:join(([1, 2], [3, 4], [5]))
(: Result: [1, 2, 3, 4, 5] :)

array:join(([1, 2], []))
(: Result: [1, 2] :)

array:join(())
(: Result: [] :)
```

## Error Codes

None specific to this function.

## See Also

- [array-flatten](array-flatten.md)
- [array-size](array-size.md)

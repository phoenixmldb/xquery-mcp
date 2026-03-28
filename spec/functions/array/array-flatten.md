---
name: array-flatten
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-array-flatten
---

# array:flatten

Recursively flattens arrays into a sequence of non-array items.

## Signature

`array:flatten($input as item()*) as item()*`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$input` | `item()*` | The input (may contain arrays) |

## Semantics

- For each item in `$input`:
  - If the item is an array, recursively flatten its members.
  - Otherwise, include the item as-is.
- Returns a flat sequence of non-array items.

## Examples

```xquery
array:flatten([1, 2, 3])
(: Result: (1, 2, 3) :)

array:flatten([1, [2, 3], [4, [5]]])
(: Result: (1, 2, 3, 4, 5) :)

array:flatten(([1, 2], [3, 4]))
(: Result: (1, 2, 3, 4) :)

array:flatten("not-an-array")
(: Result: "not-an-array" :)

array:flatten([])
(: Result: () :)
```

## Error Codes

None specific to this function.

## See Also

- [array-join](array-join.md)
- [array-size](array-size.md)

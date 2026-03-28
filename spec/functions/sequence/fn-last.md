---
name: fn-last
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-last
---

# fn:last

Returns the context size (the number of items in the sequence being processed).

## Signature

`fn:last() as xs:integer`

## Semantics

- Returns the number of items in the sequence being filtered or processed.
- Most commonly used in predicates to access the last item.

## Examples

```xquery
(: Select last item :)
(10, 20, 30)[last()]
(: Result: 30 :)

(: Select all but last :)
("a", "b", "c", "d")[position() < last()]
(: Result: "a", "b", "c" :)

(: Last child element :)
//book[last()]/title
```

## Error Codes

- `XPDY0002` — Context item is absent

## See Also

- [fn-position](fn-position.md)

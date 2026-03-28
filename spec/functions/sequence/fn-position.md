---
name: fn-position
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-position
---

# fn:position

Returns the context position (the 1-based position of the context item within the sequence being processed).

## Signature

`fn:position() as xs:integer`

## Semantics

- Returns the position of the context item in the sequence being evaluated.
- Most commonly used in predicates, where the context position reflects the position of each item being filtered.
- Also available in XSLT `xsl:for-each` and similar processing contexts.

## Examples

```xquery
(: Positional predicate :)
(10, 20, 30, 40)[position() = 2]
(: Result: 20 :)

(: Range predicate :)
(1 to 10)[position() <= 3]
(: Result: 1, 2, 3 :)

(: Even positions :)
("a", "b", "c", "d")[position() mod 2 = 0]
(: Result: "b", "d" :)

(: Combined with last() :)
//item[position() = last()]
```

## Error Codes

- `XPDY0002` — Context item is absent

## See Also

- [fn-last](fn-last.md)
- [focus](../../concepts/focus.md)

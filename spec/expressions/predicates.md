---
name: predicates
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-predicates
---

# Predicates

Predicates filter a sequence by evaluating a boolean expression for each item. They are enclosed in square brackets `[` `]` and can be applied to path steps and other expressions.

## Syntax

```
PredicateList ::= Predicate*
Predicate ::= "[" Expr "]"
```

## Semantics

A predicate filters the items in a sequence. For each item in the sequence, the predicate expression is evaluated with:
- The context item (`.`) set to the current item
- The context position (`position()`) set to the 1-based position
- The context size (`last()`) set to the total number of items

If the predicate expression evaluates to a **numeric** value, it is treated as a positional predicate: the item is retained if `position() = value`.

Otherwise, the effective boolean value (EBV) of the result determines whether the item is retained.

Multiple predicates are applied left-to-right, each filtering the result of the previous predicate.

## Examples

```xquery
(: Boolean predicate :)
//book[price < 20]

(: Positional predicate :)
//book[1]
(: Equivalent to: //book[position() = 1] :)

(: Last item :)
//item[last()]

(: Existence test :)
//order[item]
(: Selects orders that have at least one item child :)

(: Multiple predicates :)
//book[author = "Doe"][price < 30]

(: Predicate with function :)
//name[starts-with(., "A")]

(: Predicate on non-path expression :)
(1 to 100)[. mod 7 = 0]
(: Result: 7, 14, 21, ... 98 :)

(: Nested predicates :)
//chapter[section[title = "Introduction"]]

(: Positional range :)
//item[position() >= 2 and position() <= 5]
```

## Error Codes

- `FORG0006` — Invalid argument to `fn:boolean` when computing the EBV
- `XPDY0002` — Context item is absent

## See Also

- [path](path.md)
- [focus](../concepts/focus.md)
- [effective-boolean-value](../concepts/effective-boolean-value.md)

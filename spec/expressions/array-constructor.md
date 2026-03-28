---
name: array-constructor
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-array-constructors
---

# Array Constructor

Array constructors create new arrays. XQuery 3.1 provides two forms: square array constructors and curly array constructors.

## Syntax

```
SquareArrayConstructor ::= "[" (ExprSingle ("," ExprSingle)*)? "]"
CurlyArrayConstructor  ::= "array" "{" Expr? "}"
```

## Semantics

- **Square array constructor** `[E1, E2, E3]` — Each comma-separated expression becomes one member of the array. The number of members equals the number of expressions.
- **Curly array constructor** `array { E }` — The expression is evaluated and each item in the resulting sequence becomes one member of the array. The number of members equals the number of items.

Arrays are immutable, ordered collections of values. Each member can be any XQuery value. Arrays are a type of function: calling an array with an integer index returns the member at that position (1-based).

## Examples

```xquery
(: Square array — 3 members :)
[1, 2, 3]

(: Curly array — one member per item in the sequence :)
array { 1 to 5 }
(: Result: [1, 2, 3, 4, 5] :)

(: Difference between square and curly :)
[(1, 2, 3)]
(: Result: array with 1 member (the sequence 1,2,3) :)

array { (1, 2, 3) }
(: Result: array with 3 members :)

(: Nested arrays :)
[[1, 2], [3, 4]]

(: Array of maps :)
[map{"name": "Alice"}, map{"name": "Bob"}]

(: Empty arrays :)
[]
array {}

(: Array as a function :)
let $a := ["x", "y", "z"]
return $a(2)
(: Result: "y" :)

(: Array from query results :)
array { for $book in //book return $book/title/string() }
```

## Error Codes

- `FOAY0001` — Array index out of bounds when accessing a member
- `FOAY0002` — Negative array length (in array construction functions)

## See Also

- [map-constructor](map-constructor.md)
- [lookup-operator](lookup-operator.md)
- [array-size](../functions/array/array-size.md)

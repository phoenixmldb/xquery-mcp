---
name: lookup-operator
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-lookup
---

# Lookup Operator

The lookup operator `?` provides concise access to entries in maps and members of arrays.

## Syntax

```
PostfixExpr ::= PrimaryExpr (Predicate | ArgumentList | Lookup)*
Lookup ::= "?" KeySpecifier
KeySpecifier ::= NCName | IntegerLiteral | ParenthesizedExpr | "*"
UnaryLookup ::= "?" KeySpecifier
```

## Semantics

The lookup operator `?` is used with maps and arrays:

- **Map lookup**: `$map?key` returns the value associated with the key. Equivalent to `$map("key")` or `map:get($map, "key")`.
- **Array lookup**: `$array?N` returns the Nth member (1-based). Equivalent to `$array(N)` or `array:get($array, N)`.
- **Wildcard lookup**: `$map?*` returns all values in the map. `$array?*` returns all array members.
- **Computed lookup**: `$map?(expr)` evaluates the expression to determine the key.

The **unary lookup** `?key` is shorthand for `.?key` and is used in predicates and other contexts where the context item is a map or array.

When applied to a sequence, the lookup is applied to each item and the results are concatenated.

## Examples

```xquery
(: Map lookup :)
let $map := map { "name": "Alice", "age": 30 }
return $map?name
(: Result: "Alice" :)

(: Array lookup :)
let $arr := [10, 20, 30]
return $arr?2
(: Result: 20 :)

(: Wildcard lookup :)
let $map := map { "a": 1, "b": 2, "c": 3 }
return $map?*
(: Result: 1, 2, 3 (in implementation-defined order) :)

(: Chained lookup :)
let $data := map { "users": [map{"name":"Alice"}, map{"name":"Bob"}] }
return $data?users?*?name
(: Result: "Alice", "Bob" :)

(: Computed key :)
let $map := map { "x": 1, "y": 2 }
let $key := "x"
return $map?($key)

(: Unary lookup in predicate :)
let $maps := (map{"a":1}, map{"a":2}, map{"a":3})
return $maps[?a > 1]
(: Result: map{"a":2}, map{"a":3} :)
```

## Error Codes

- `XPTY0004` — Context item is not a map or array when using the lookup operator
- `FOAY0001` — Array index out of bounds
- `XPDY0002` — Context item is absent (for unary lookup)

## See Also

- [map-constructor](map-constructor.md)
- [array-constructor](array-constructor.md)

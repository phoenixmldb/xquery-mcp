---
name: map-constructor
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-map-constructors
---

# Map Constructor

The map constructor creates a new map from a set of key-value pairs.

## Syntax

```
MapConstructor ::= "map" "{" (MapConstructorEntry ("," MapConstructorEntry)*)? "}"
MapConstructorEntry ::= MapKeyExpr ":" MapValueExpr
MapKeyExpr ::= ExprSingle
MapValueExpr ::= ExprSingle
```

## Semantics

A map is an immutable collection of key-value pairs where each key is a unique atomic value. Maps are a type of function: calling a map as a function with a key returns the associated value.

- Keys must be atomic values. They are compared using the `eq` operator.
- Values can be any XQuery value (atomic values, nodes, sequences, maps, arrays, functions).
- Duplicate keys raise a dynamic error.
- The empty map `map {}` contains no entries.
- Maps can be nested and combined with arrays.

## Examples

```xquery
(: Simple map :)
map { "name": "Alice", "age": 30 }

(: Map with various key types :)
map { 1: "one", 2: "two", 3: "three" }

(: Nested maps :)
map {
  "address": map {
    "street": "123 Main St",
    "city": "Springfield"
  }
}

(: Map with sequence values :)
map { "colors": ("red", "green", "blue") }

(: Map with computed keys and values :)
map {
  $key1: $value1,
  concat("key", "2"): 2 + 3
}

(: Using map as a function :)
let $m := map { "x": 1, "y": 2 }
return $m("x")
(: Result: 1 :)

(: Empty map :)
map {}

(: Map with array values :)
map {
  "matrix": [[1, 2], [3, 4]]
}
```

## Error Codes

- `XQDY0137` — Duplicate key in map constructor

## See Also

- [array-constructor](array-constructor.md)
- [lookup-operator](lookup-operator.md)
- [map-merge](../functions/map/map-merge.md)

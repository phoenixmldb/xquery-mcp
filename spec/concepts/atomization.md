---
name: atomization
category: concept
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-atomization
---

# Atomization

Atomization is the process of converting a value to a sequence of atomic values. It is applied implicitly in many contexts, such as function arguments, comparisons, and arithmetic.

## Semantics

Atomization is defined by `fn:data()` and works as follows:

| Input | Result |
|-------|--------|
| Atomic value | Returned unchanged |
| Node | The typed value of the node (usually `xs:untypedAtomic` for untyped nodes, or the string value) |
| Array | Each member is atomized recursively |
| Map | Error `FOTY0013` — maps cannot be atomized |
| Function | Error `FOTY0013` — functions cannot be atomized |

For element nodes without schema types, atomization returns the string value as `xs:untypedAtomic`.

## Where Atomization Occurs Implicitly

- Arithmetic expressions (`+`, `-`, `*`, `div`, `mod`, `idiv`)
- Comparison expressions (`=`, `!=`, `<`, `>`, `eq`, `ne`, `lt`, `gt`, etc.)
- Function arguments (when the function expects atomic types)
- `switch` operand
- `group by` keys
- `order by` sort keys
- Cast expressions
- String concatenation (`||`)

## Examples

```xquery
(: Atomization of an element :)
let $e := <price>9.99</price>
return $e + 1
(: $e is atomized to "9.99" (xs:untypedAtomic), then promoted to xs:double :)
(: Result: 10.99 :)

(: Explicit atomization with fn:data :)
fn:data(<name>Alice</name>)
(: Result: xs:untypedAtomic("Alice") :)

(: Atomization of a sequence :)
fn:data((<a>1</a>, <b>2</b>))
(: Result: (xs:untypedAtomic("1"), xs:untypedAtomic("2")) :)

(: Array atomization :)
fn:data([1, "hello", <x>42</x>])
(: Result: (1, "hello", xs:untypedAtomic("42")) :)
```

## Error Codes

- `FOTY0012` — Atomization applied to a function item
- `FOTY0013` — Atomization applied to a function item (3.1 variant)

## See Also

- [effective-boolean-value](effective-boolean-value.md)
- [fn-string](../functions/string/fn-string.md)

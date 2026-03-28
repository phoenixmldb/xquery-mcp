---
name: for
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-for
---

# For Clause

The `for` clause iterates over a sequence, binding a variable to each item in turn. It is one of the initial clauses of a FLWOR expression.

## Syntax

```
ForClause ::= "for" ForBinding ("," ForBinding)*
ForBinding ::= "$" VarName TypeDeclaration? AllowingEmpty? PositionalVar? "in" ExprSingle
AllowingEmpty ::= "allowing" "empty"
PositionalVar ::= "at" "$" VarName
```

## Semantics

For each item in the binding sequence, the `for` clause generates one tuple in the tuple stream with the variable bound to that item. If a positional variable is specified with `at`, it is bound to the 1-based position of the current item.

When `allowing empty` is specified, if the binding sequence is empty, one tuple is generated with the variable bound to the empty sequence (similar to an outer join). Without `allowing empty`, an empty binding sequence produces no tuples.

Multiple `for` bindings separated by commas produce a Cartesian product.

## Examples

```xquery
(: Simple for :)
for $x in (1, 2, 3)
return $x * $x

(: For with positional variable :)
for $name at $pos in ("Alice", "Bob", "Charlie")
return concat($pos, ". ", $name)

(: For with allowing empty :)
for $child allowing empty in $parent/child
return
  if (empty($child))
  then <no-children/>
  else <child>{$child}</child>

(: Multiple for bindings :)
for $x in (1, 2), $y in (10, 20)
return $x + $y
(: Result: 11, 21, 12, 22 :)

(: For with type declaration :)
for $price as xs:decimal in //product/price
return $price * 1.1
```

## Error Codes

- `XPTY0004` — Type error if the binding expression result does not match the declared type
- `XPST0008` — Undefined variable reference within the for clause

## See Also

- [flwor](flwor.md)
- [let](let.md)

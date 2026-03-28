---
name: order-by
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-orderby-return
---

# Order By Clause

The `order by` clause sorts the tuple stream in a FLWOR expression.

## Syntax

```
OrderByClause ::= (("order" "by") | ("stable" "order" "by")) OrderSpecList
OrderSpecList ::= OrderSpec ("," OrderSpec)*
OrderSpec ::= ExprSingle OrderModifier
OrderModifier ::= ("ascending" | "descending")?
                   ("empty" ("greatest" | "least"))?
                   ("collation" URILiteral)?
```

## Semantics

The `order by` clause reorders the tuples in the tuple stream based on one or more ordering specifications. Each ordering spec is evaluated for every tuple, and the resulting values are used as sort keys.

- **ascending** (default) or **descending** — controls sort direction.
- **empty greatest** or **empty least** — controls where empty sequences and NaN values are sorted. The default is implementation-defined.
- **stable order by** — guarantees that tuples with equal sort keys retain their original relative order.
- **collation** — specifies the collation URI for string comparison.

Values are compared using the `gt` operator rules. Atomic values of different types are compared after type promotion.

## Examples

```xquery
(: Simple ascending order :)
for $x in (3, 1, 4, 1, 5)
order by $x
return $x

(: Descending order :)
for $book in //book
order by $book/price descending
return $book/title

(: Multiple sort keys :)
for $emp in //employee
order by $emp/department ascending, $emp/salary descending
return $emp

(: Stable order by :)
for $item in //item
stable order by $item/category
return $item

(: Handling empty values :)
for $product in //product
order by $product/rating descending empty least
return $product/name
```

## Error Codes

- `XPTY0004` — Type error in an order-by expression (e.g., comparing incompatible types)

## See Also

- [flwor](flwor.md)

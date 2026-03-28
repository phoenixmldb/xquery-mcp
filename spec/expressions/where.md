---
name: where
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-where
---

# Where Clause

The `where` clause filters the tuple stream in a FLWOR expression, retaining only tuples for which the condition evaluates to true.

## Syntax

```
WhereClause ::= "where" ExprSingle
```

## Semantics

The `where` clause evaluates its expression for each tuple in the tuple stream. The expression result is converted to a boolean value using the Effective Boolean Value (EBV) rules. Tuples for which the EBV is `true` are retained; others are discarded.

## Examples

```xquery
(: Simple where :)
for $x in 1 to 10
where $x mod 2 = 0
return $x

(: Where with node test :)
for $book in //book
where $book/price < 20
return $book/title

(: Where with multiple conditions :)
for $emp in //employee
where $emp/department = "Engineering"
  and $emp/salary > 80000
return $emp/name

(: Where with existential test :)
for $order in //order
where $order/item[quantity > 100]
return $order/id

(: Where with function call :)
for $name in //customer/name
where starts-with($name, "A")
return $name
```

## Error Codes

- `FORG0006` — Invalid argument to `fn:boolean` when computing the EBV (e.g., a sequence of more than one item starting with an atomic value)

## See Also

- [flwor](flwor.md)
- [effective-boolean-value](../concepts/effective-boolean-value.md)

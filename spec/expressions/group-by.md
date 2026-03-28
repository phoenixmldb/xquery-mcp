---
name: group-by
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-group-by
---

# Group By Clause

The `group by` clause partitions the tuple stream into groups based on the values of one or more grouping variables.

## Syntax

```
GroupByClause ::= "group" "by" GroupingSpecList
GroupingSpecList ::= GroupingSpec ("," GroupingSpec)*
GroupingSpec ::= GroupingVariable (TypeDeclaration? ":=" ExprSingle)?
                 ("collation" URILiteral)?
GroupingVariable ::= "$" VarName
```

## Semantics

The `group by` clause partitions tuples in the tuple stream into groups sharing the same value(s) for the grouping variable(s). After grouping:

- Each **grouping variable** is bound to a single value (the common value for the group).
- Each **non-grouping variable** is bound to the **sequence** of all its values across the tuples in the group.

Values are compared for grouping equality using the `eq` operator. Empty sequences form their own group. NaN values are grouped together.

## Examples

```xquery
(: Basic group by :)
for $sale in //sale
group by $year := year-from-date($sale/date)
return <year value="{$year}">{sum($sale/amount)}</year>

(: Group by existing variable :)
for $emp in //employee
let $dept := $emp/department/string()
group by $dept
return <department name="{$dept}" count="{count($emp)}"/>

(: Multiple grouping keys :)
for $order in //order
group by $year := year-from-date($order/date),
         $status := $order/status/string()
return <summary year="{$year}" status="{$status}" count="{count($order)}"/>

(: Group by with collation :)
for $word in tokenize($text, "\s+")
group by $lower := lower-case($word)
return <word value="{$lower}" count="{count($word)}"/>

(: Accessing non-grouping variables as sequences :)
for $book in //book
group by $genre := $book/genre/string()
order by $genre
return
  <genre name="{$genre}">
    <count>{count($book)}</count>
    <titles>{string-join($book/title, ", ")}</titles>
  </genre>
```

## Error Codes

- `XPTY0004` — Type error in grouping specification

## See Also

- [flwor](flwor.md)
- [order-by](order-by.md)

---
name: flwor
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-flwor-expressions
---

# FLWOR Expression

The FLWOR expression is the most powerful expression in XQuery, supporting iteration, binding, filtering, ordering, grouping, windowing, and counting. The name is an acronym for **F**or, **L**et, **W**here, **O**rder by, **R**eturn.

## Syntax

```
FLWORExpr ::= InitialClause IntermediateClause* ReturnClause
InitialClause ::= ForClause | LetClause | WindowClause
IntermediateClause ::= InitialClause | WhereClause | GroupByClause
                     | OrderByClause | CountClause
ReturnClause ::= "return" ExprSingle
```

## Semantics

A FLWOR expression begins with one or more `for` or `let` clauses (the initial clause), followed by zero or more intermediate clauses (`for`, `let`, `where`, `order by`, `group by`, `count`, `window`), and ends with a `return` clause.

The evaluation produces a **tuple stream** — a sequence of tuples of variable bindings that flows through each clause. Each clause transforms the tuple stream before passing it to the next clause.

## Examples

```xquery
(: Basic FLWOR with for, where, order by, return :)
for $book in //book
where $book/year > 2000
order by $book/title
return $book/title

(: Multiple for clauses (Cartesian product) :)
for $x in (1, 2, 3)
for $y in ("a", "b")
return concat($x, $y)

(: Combining for and let :)
for $dept in distinct-values(//employee/department)
let $emps := //employee[department = $dept]
let $avg-salary := avg($emps/salary)
where $avg-salary > 50000
order by $avg-salary descending
return
  <department name="{$dept}">
    <avg-salary>{$avg-salary}</avg-salary>
    <count>{count($emps)}</count>
  </department>

(: FLWOR with group by :)
for $sale in //sale
group by $year := year-from-date($sale/date)
order by $year
return
  <yearly-total year="{$year}">{sum($sale/amount)}</yearly-total>

(: FLWOR with count :)
for $item in //item
order by $item/name
count $pos
return <numbered-item pos="{$pos}">{$item/name/string()}</numbered-item>
```

## Error Codes

- `XPTY0004` — Type error in a clause (e.g., non-boolean where clause after atomization)
- `XQST0089` — Duplicate variable binding in the same `for`/`let` clause (static error)

## See Also

- [for](for.md)
- [let](let.md)
- [where](where.md)
- [order-by](order-by.md)
- [group-by](group-by.md)
- [return](return.md)

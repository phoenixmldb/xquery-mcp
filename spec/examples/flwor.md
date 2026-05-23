---
name: flwor
category: example
since: "1.0"
spec_url: https://www.w3.org/TR/xquery-31/#id-flwor-expressions
---

# FLWOR expression — for, let, where, order by, group by, return

## Basic FLWOR with filtering and ordering

```xquery
xquery version "3.1";

let $catalog := doc('catalog.xml')
for $item in $catalog//item
where xs:decimal($item/@price) gt 10.00
order by $item/@name ascending
return
  <item name="{$item/@name}" price="{$item/@price}"/>
```

## Group-by clause (XQuery 3.0+)

```xquery
xquery version "3.1";

let $orders := doc('orders.xml')//order
for $o in $orders
let $cat := string($o/@category)
group by $cat
order by $cat
return
  <category name="{$cat}" total="{count($o)}"
            revenue="{sum($o/xs:decimal(@amount))}"/>
```

## Nested FLWOR with let binding

```xquery
xquery version "3.1";

for $dept in doc('org.xml')//department
let $employees := $dept/employee
let $avg-salary := avg($employees/xs:decimal(@salary))
where count($employees) gt 0
return
  <dept name="{$dept/@name}"
        headcount="{count($employees)}"
        avg-salary="{format-number($avg-salary, '#.00')}">
    {
      for $emp in $employees
      order by xs:decimal($emp/@salary) descending
      return $emp
    }
  </dept>
```

## What it does

The FLWOR expression is XQuery's primary iteration and transformation
construct. Clauses execute top-to-bottom:

- `for` — binds a variable to each item in a sequence (introduces a tuple per item)
- `let` — binds a variable to an entire sequence (single tuple extension)
- `where` — filters tuples
- `group by` — collapses tuples by a grouping key; `$var` becomes a sequence within each group
- `order by` — sorts remaining tuples
- `return` — evaluated once per surviving tuple; its results are concatenated

## Common pitfalls

- `let` does **not** iterate — `let $x := (1, 2, 3)` binds `$x` to the
  whole sequence, not three separate tuples. Use `for` to iterate.
- After `group by`, the grouped variable holds a **sequence** of all items
  in the group. Forgetting this causes `count($o)` to always return 1.
- `order by` sorts tuples **before** `return` — reordering inside `return`
  requires a nested `for`.
- XQuery uses `gt`, `lt`, `eq` for value comparison, not `>`, `<`, `=`
  (those are general comparisons with different semantics for sequences).

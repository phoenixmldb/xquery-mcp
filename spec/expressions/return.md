---
name: return
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-orderby-return
---

# Return Clause

The `return` clause is the final clause of a FLWOR expression. It specifies the expression to evaluate for each tuple in the tuple stream, producing the result of the FLWOR expression.

## Syntax

```
ReturnClause ::= "return" ExprSingle
```

## Semantics

The `return` clause evaluates its expression once for each tuple remaining in the tuple stream after all preceding clauses have been processed. The results are concatenated in order to form the result of the entire FLWOR expression.

The return expression has access to all variables bound by preceding `for`, `let`, `group by`, `window`, and `count` clauses.

## Examples

```xquery
(: Simple return :)
for $x in 1 to 5
return $x * $x

(: Return with element constructor :)
for $emp in //employee
return <name>{$emp/first || " " || $emp/last}</name>

(: Return with nested FLWOR :)
for $dept in distinct-values(//employee/department)
return
  <department name="{$dept}">
  {
    for $emp in //employee[department = $dept]
    order by $emp/name
    return <employee>{$emp/name/string()}</employee>
  }
  </department>
```

## See Also

- [flwor](flwor.md)

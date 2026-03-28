---
name: if-then-else
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-conditionals
---

# If-Then-Else Expression

The conditional expression evaluates one of two branches based on the effective boolean value of a test expression.

## Syntax

```
IfExpr ::= "if" "(" Expr ")" "then" ExprSingle "else" ExprSingle
```

In XQuery 4.0, the `else` branch is optional and defaults to `()`:

```
IfExpr ::= "if" "(" Expr ")" "then" ExprSingle ("else" ExprSingle)?
```

## Semantics

1. The test expression (inside parentheses) is evaluated.
2. Its effective boolean value (EBV) is computed.
3. If the EBV is `true`, the `then` expression is evaluated and its value is the result.
4. If the EBV is `false`, the `else` expression is evaluated and its value is the result.

Only one branch is evaluated (short-circuit evaluation).

## Examples

```xquery
(: Simple conditional :)
if ($x > 0) then "positive" else "non-positive"

(: Conditional with node test :)
if ($order/priority = "high")
then <urgent>{$order}</urgent>
else $order

(: Nested conditionals :)
if ($score >= 90) then "A"
else if ($score >= 80) then "B"
else if ($score >= 70) then "C"
else "F"

(: Conditional with existence test :)
if ($node/child)
then $node/child
else <default/>

(: XQuery 4.0 — else is optional :)
if ($debug) then trace($value, "debug")
```

## Error Codes

- `FORG0006` — Invalid argument to `fn:boolean` when computing the EBV of the test expression

## See Also

- [switch](switch.md)
- [typeswitch](typeswitch.md)
- [effective-boolean-value](../concepts/effective-boolean-value.md)

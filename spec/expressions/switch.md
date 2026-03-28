---
name: switch
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-switch
---

# Switch Expression

The `switch` expression selects one of several branches based on the value of an operand expression, similar to a switch/case statement in other languages.

## Syntax

```
SwitchExpr ::= "switch" "(" Expr ")"
               SwitchCaseClause+
               "default" "return" ExprSingle
SwitchCaseClause ::= ("case" SwitchCaseOperand)+ "return" ExprSingle
SwitchCaseOperand ::= ExprSingle
```

## Semantics

1. The operand expression is evaluated and atomized.
2. If the result is a sequence of more than one item, a type error is raised.
3. The atomized value is compared to each case operand using the `eq` operator (with no collation for strings — uses the default collation).
4. The first matching case's return expression is evaluated.
5. If no case matches, the `default` return expression is evaluated.

A case operand that evaluates to the empty sequence matches the operand only if the operand is also the empty sequence.

Multiple `case` labels can share a single `return` expression.

## Examples

```xquery
(: Basic switch :)
switch ($animal)
  case "cat" return "meow"
  case "dog" return "woof"
  case "cow" return "moo"
  default return "unknown"

(: Multiple case labels :)
switch ($day)
  case "Monday"
  case "Tuesday"
  case "Wednesday"
  case "Thursday"
  case "Friday" return "weekday"
  case "Saturday"
  case "Sunday" return "weekend"
  default return "invalid"

(: Switch on computed value :)
switch (count($items))
  case 0 return "empty"
  case 1 return "single"
  default return "multiple"
```

## Error Codes

- `XPTY0004` — Type error if the operand atomizes to a sequence of more than one item

## See Also

- [if-then-else](if-then-else.md)
- [typeswitch](typeswitch.md)

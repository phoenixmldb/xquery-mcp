---
name: typeswitch
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-typeswitch
---

# Typeswitch Expression

The `typeswitch` expression selects a branch based on the dynamic type of an operand expression.

## Syntax

```
TypeswitchExpr ::= "typeswitch" "(" Expr ")"
                   CaseClause+
                   "default" ("$" VarName)? "return" ExprSingle
CaseClause ::= "case" ("$" VarName "as")? SequenceType
               ("union" SequenceType)*  (: XQuery 4.0 uses | :)
               "return" ExprSingle
```

## Semantics

1. The operand expression is evaluated.
2. The result is matched against each `case` clause's sequence type in order, using the `instance of` operator.
3. The first matching case's return expression is evaluated. If a variable is declared in the case, it is bound to the operand value.
4. If no case matches, the `default` return expression is evaluated.

In XQuery 3.1, multiple types in a single case use the `union` keyword. In XQuery 4.0, the `|` operator can be used instead.

## Examples

```xquery
(: Basic typeswitch :)
typeswitch ($value)
  case xs:integer return "integer"
  case xs:string return "string"
  case xs:boolean return "boolean"
  default return "other"

(: With variable binding :)
typeswitch ($node)
  case $e as element(book) return $e/title
  case $e as element(author) return $e/name
  case $a as attribute() return string($a)
  case $t as text() return $t
  default $other return "unknown"

(: Union types in case :)
typeswitch ($item)
  case xs:integer union xs:decimal return "numeric"
  case xs:string return "text"
  default return "other"

(: Matching sequence types :)
typeswitch ($seq)
  case empty-sequence() return "empty"
  case xs:integer+ return "integers"
  case node()+ return "nodes"
  default return "mixed"
```

## Error Codes

- `XPST0003` — Syntax error in a sequence type
- `XPST0051` — Unknown atomic type in a sequence type

## See Also

- [if-then-else](if-then-else.md)
- [switch](switch.md)

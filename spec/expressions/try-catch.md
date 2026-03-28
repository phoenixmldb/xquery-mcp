---
name: try-catch
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-try-catch
---

# Try-Catch Expression

The `try-catch` expression provides error handling by catching dynamic errors raised during evaluation of the try expression.

## Syntax

```
TryCatchExpr ::= TryClause CatchClause+
TryClause ::= "try" "{" TryTargetExpr "}"
CatchClause ::= "catch" CatchErrorList "{" Expr "}"
CatchErrorList ::= NameTest ("|" NameTest)*
```

## Semantics

1. The try expression is evaluated.
2. If a dynamic error occurs, the error code is matched against each catch clause's error list.
3. The first matching catch clause is evaluated. Within the catch clause, the following variables are implicitly bound:
   - `$err:code` — the error code as `xs:QName`
   - `$err:description` — the error description as `xs:string?`
   - `$err:value` — the error value as `item()*`
   - `$err:module` — the module URI as `xs:string?`
   - `$err:line-number` — the line number as `xs:integer?`
   - `$err:column-number` — the column number as `xs:integer?`
   - `$err:additional` — additional implementation-defined information as `item()*`

The namespace `http://www.w3.org/2005/xqt-errors` must be bound to the prefix `err` for these variables.

A wildcard `*` in the catch clause matches any error code.

## Examples

```xquery
(: Catch any error :)
try {
  1 div 0
} catch * {
  "Division by zero: " || $err:description
}

(: Catch specific error codes :)
try {
  xs:integer("not-a-number")
} catch err:FORG0001 {
  "Invalid cast"
} catch err:XPTY0004 {
  "Type error"
} catch * {
  "Other error: " || $err:code
}

(: Multiple error codes in one catch :)
try {
  $doc//element
} catch err:FODC0002 | err:FODC0005 {
  "Document access error"
}

(: Using error variables :)
try {
  some-function($input)
} catch * {
  <error>
    <code>{$err:code}</code>
    <message>{$err:description}</message>
    <module>{$err:module}</module>
    <line>{$err:line-number}</line>
  </error>
}
```

## Error Codes

- Static errors and type errors are not catchable by try-catch.
- Only dynamic errors raised during the evaluation of the try expression are caught.

## See Also

- [error-codes](../error-codes/)

---
name: let
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-let
---

# Let Clause

The `let` clause binds a variable to the value of an expression. Unlike `for`, it does not iterate — it binds the entire result sequence to the variable.

## Syntax

```
LetClause ::= "let" LetBinding ("," LetBinding)*
LetBinding ::= "$" VarName TypeDeclaration? ":=" ExprSingle
```

## Semantics

The `let` clause evaluates its expression and binds the result (which may be a sequence of any length, including empty) to the variable. Each existing tuple in the tuple stream is augmented with the new variable binding. The number of tuples remains unchanged.

## Examples

```xquery
(: Simple let :)
let $x := 42
return $x * 2

(: Let binding a sequence :)
let $nums := (1, 2, 3, 4, 5)
return sum($nums)

(: Let with type declaration :)
let $name as xs:string := "hello"
return upper-case($name)

(: Multiple let bindings :)
let $first := "John",
    $last := "Doe"
return concat($first, " ", $last)

(: Let combined with for :)
for $dept in distinct-values(//employee/department)
let $count := count(//employee[department = $dept])
return <dept name="{$dept}" count="{$count}"/>
```

## Error Codes

- `XPTY0004` — Type error if the expression result does not match the declared type

## See Also

- [flwor](flwor.md)
- [for](for.md)

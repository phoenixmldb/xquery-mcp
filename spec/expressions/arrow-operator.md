---
name: arrow-operator
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-arrow-operator
---

# Arrow Operator

The arrow operator `=>` provides a pipeline syntax for chaining function calls, passing the left-hand expression as the first argument to the right-hand function.

## Syntax

```
ArrowExpr ::= UnaryExpr ("=>" ArrowFunctionSpecifier ArgumentList)*
ArrowFunctionSpecifier ::= EQName | VarRef | ParenthesizedExpr
```

In XQuery 4.0, the **thin arrow** `->` is also available, which passes the left-hand side as the context item (`.`) rather than as the first argument:

```
ThinArrowExpr ::= UnaryExpr ("->" ArrowFunctionSpecifier ArgumentList)*
```

## Semantics

The expression `E => F(args)` is equivalent to `F(E, args)`. The left operand becomes the first argument of the function call, and any explicit arguments follow.

The arrow operator is left-associative, allowing chains: `E => F() => G()` means `G(F(E))`.

In XQuery 4.0, `E -> F()` evaluates `F()` with `.` bound to `E`.

## Examples

```xquery
(: Basic arrow — equivalent to upper-case("hello") :)
"hello" => upper-case()

(: Chaining — equivalent to tokenize(normalize-space("  a  b  c  "), " ") :)
"  a  b  c  " => normalize-space() => tokenize(" ")

(: Arrow with additional arguments :)
"hello world" => substring(1, 5)
(: Equivalent to: substring("hello world", 1, 5) :)

(: Arrow with variable reference :)
let $f := function($x) { $x * 2 }
return 21 => $f()

(: Complex pipeline :)
$doc//employee
  => filter(function($e) { $e/salary > 50000 })
  => sort((), function($e) { $e/name })
  => for-each(function($e) { $e/name/string() })

(: XQuery 4.0 thin arrow :)
"hello world" -> tokenize() -> count()
```

## Error Codes

- `XPTY0004` — Type error when the left operand is not compatible with the function's first parameter
- `XPST0017` — Function not found

## See Also

- [lookup-operator](lookup-operator.md)

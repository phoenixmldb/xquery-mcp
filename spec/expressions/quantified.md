---
name: quantified
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-quantified-expressions
---

# Quantified Expressions

Quantified expressions test whether `some` or `every` item in a sequence satisfies a condition. They return a boolean value.

## Syntax

```
QuantifiedExpr ::= ("some" | "every") "$" VarName "in" ExprSingle
                   ("," "$" VarName "in" ExprSingle)*
                   "satisfies" ExprSingle
```

## Semantics

- **`some`** — Returns `true` if at least one binding satisfies the test expression (existential quantification).
- **`every`** — Returns `true` if all bindings satisfy the test expression (universal quantification).

The satisfies expression is converted to its effective boolean value. Multiple variable bindings produce a Cartesian product (all combinations are tested).

For `every`, if any binding sequence is empty, the result is `true` (vacuous truth). For `some`, if all binding sequences are empty, the result is `false`.

## Examples

```xquery
(: Some — existential :)
some $x in (1, 2, 3) satisfies $x > 2
(: Result: true :)

(: Every — universal :)
every $x in (2, 4, 6) satisfies $x mod 2 = 0
(: Result: true :)

(: With node test :)
some $book in //book satisfies $book/price < 10

(: Multiple variables :)
some $x in (1, 2, 3), $y in (4, 5, 6)
satisfies $x + $y = 7
(: Result: true (e.g. 1+6, 2+5, 3+4) :)

(: Vacuous truth with every :)
every $x in () satisfies false()
(: Result: true :)
```

## Error Codes

- `FORG0006` — Invalid argument to `fn:boolean` when computing EBV of the satisfies expression

## See Also

- [flwor](flwor.md)
- [effective-boolean-value](../concepts/effective-boolean-value.md)

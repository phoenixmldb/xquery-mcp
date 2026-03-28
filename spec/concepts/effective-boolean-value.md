---
name: effective-boolean-value
category: concept
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-ebv
---

# Effective Boolean Value (EBV)

The effective boolean value is the boolean interpretation of a value, used implicitly in `if`, `where`, `while`, predicates, and logical operators (`and`, `or`).

## Rules

The effective boolean value is computed by applying `fn:boolean()`:

| Input | EBV |
|-------|-----|
| Empty sequence `()` | `false` |
| Sequence starting with a node | `true` |
| Single `xs:boolean` | The boolean value itself |
| Single `xs:string` or `xs:untypedAtomic` | `false` if zero-length, `true` otherwise |
| Single `xs:anyURI` | `false` if zero-length, `true` otherwise |
| Single numeric (`xs:integer`, `xs:decimal`, `xs:float`, `xs:double`) | `false` if `0`, `+0`, `-0`, or `NaN`; `true` otherwise |
| Sequence of two or more items starting with an atomic value | **Error** `FORG0006` |
| Single function item (map, array, or function) | **Error** `FORG0006` |

## Where EBV Is Applied

- `if` test expression
- `where` clause in FLWOR
- Predicate expressions `[...]`
- `and`, `or` operators
- `some`/`every` satisfies expression
- `fn:boolean()`, `fn:not()`
- `while` (XQuery 4.0)

## Examples

```xquery
(: Boolean :)
if (true()) then "yes" else "no"         (: "yes" :)

(: Number — 0 is false :)
if (0) then "yes" else "no"              (: "no" :)
if (42) then "yes" else "no"             (: "yes" :)

(: String — empty is false :)
if ("") then "yes" else "no"             (: "no" :)
if ("hello") then "yes" else "no"        (: "yes" :)

(: Empty sequence — false :)
if (()) then "yes" else "no"             (: "no" :)

(: Node — true :)
if (<x/>) then "yes" else "no"           (: "yes" :)

(: Common pattern — existence test :)
if (//book) then "found" else "none"

(: Error case :)
if ((1, 2)) then "yes" else "no"
(: Error: FORG0006 :)
```

## Error Codes

- `FORG0006` — Cannot compute EBV (sequence of two or more atomic values, or a function item)

## See Also

- [atomization](atomization.md)
- [fn-boolean](../functions/boolean/fn-boolean.md)

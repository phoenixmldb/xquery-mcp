---
name: string-constructors
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-string-constructors
---

# String Constructors

String constructors provide a way to create strings with embedded expressions using backtick syntax, avoiding the need to escape quotes and special characters.

## Syntax

```
StringConstructor ::= "``[" StringConstructorContent "]``"
StringConstructorContent ::= StringConstructorChars
                            (StringConstructorInterpolation StringConstructorChars)*
StringConstructorInterpolation ::= "`{" Expr "}`"
```

## Semantics

A string constructor is delimited by `` ``[ `` and `` ]`` ``. Within the constructor:

- Literal text is preserved as-is, including newlines, quotes, and special characters.
- Expressions are interpolated using `` `{ `` and `` }` `` delimiters.
- Interpolated expressions are atomized and cast to `xs:string`, with items separated by spaces.

String constructors are particularly useful for generating code, JSON, or other text formats where quotes and braces appear frequently.

## Examples

```xquery
(: Simple string constructor :)
``[Hello, World!]``
(: Result: "Hello, World!" :)

(: With interpolation :)
let $name := "Alice"
return ``[Hello, `{$name}`!]``
(: Result: "Hello, Alice!" :)

(: Preserving special characters :)
``[She said "hello" and he said 'goodbye'.]``

(: Generating JSON :)
let $name := "widget"
let $price := 9.99
return ``[{"name": "`{$name}`", "price": `{$price}`}]``
(: Result: {"name": "widget", "price": 9.99} :)

(: Multi-line strings :)
``[Line 1
Line 2
Line 3]``

(: Generating code :)
let $var := "x"
let $val := 42
return ``[let $`{$var}` := `{$val}`
return $`{$var}` * 2]``
```

## Error Codes

- `XPTY0004` — Type error when casting interpolated expression to string

## See Also

- [constructors](constructors.md)

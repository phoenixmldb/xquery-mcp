---
name: variable-declaration
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-variable-declarations
---

# Variable Declaration

Declares a global variable in the module prolog, optionally with a type and initial value.

## Syntax

```
VarDecl ::= "declare" Annotation* "variable" "$" VarName TypeDeclaration?
            ((":=" VarValue) | ("external" (":=" VarDefaultValue)?))
            Separator
```

## Semantics

- Declares a variable that is visible throughout the module.
- Variables can be assigned a value with `:=` or declared as `external` (the value is provided by the calling environment).
- External variables may have a default value.
- Annotations can mark visibility (`%public`, `%private`) or other properties.
- The type declaration is optional but recommended.

## Examples

```xquery
(: Simple variable :)
declare variable $greeting := "Hello, World!";

(: Typed variable :)
declare variable $max-retries as xs:integer := 3;

(: External variable :)
declare variable $input-file as xs:string external;

(: External with default :)
declare variable $debug as xs:boolean external := false();

(: Private variable :)
declare %private variable $internal-state := map{};

(: Sequence variable :)
declare variable $colors := ("red", "green", "blue");
```

## Error Codes

- `XQST0049` — Duplicate variable declaration
- `XPTY0004` — Type mismatch between declared type and assigned value
- `XPDY0002` — External variable not provided and no default value

## See Also

- [function-declaration](function-declaration.md)
- [annotations](annotations.md)

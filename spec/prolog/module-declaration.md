---
name: module-declaration
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-module-declaration
---

# Module Declaration

A module declaration marks an XQuery module as a library module and associates it with a target namespace.

## Syntax

```
ModuleDecl ::= "module" "namespace" NCName "=" URILiteral Separator
```

## Semantics

- A module declaration makes the module a **library module** (as opposed to a **main module**).
- Library modules contain function and variable declarations but do not have a query body.
- The namespace prefix and URI identify the module's target namespace.
- All functions and variables declared in the module are in the target namespace.
- A module can only have one module declaration, and it must appear before all other declarations.

## Examples

```xquery
module namespace math = "http://example.com/math";

declare function math:square($x as xs:integer) as xs:integer {
  $x * $x
};

declare function math:cube($x as xs:integer) as xs:integer {
  $x * $x * $x
};
```

## Error Codes

- `XQST0048` — A function or variable in a library module is not in the module's target namespace
- `XQST0088` — The module namespace URI is a zero-length string

## See Also

- [import-module](import-module.md)
- [namespace-declaration](namespace-declaration.md)
- [function-declaration](function-declaration.md)

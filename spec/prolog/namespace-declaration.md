---
name: namespace-declaration
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-namespace-declaration
---

# Namespace Declaration

A namespace declaration binds a prefix to a namespace URI for use throughout the module.

## Syntax

```
NamespaceDecl ::= "declare" "namespace" NCName "=" URILiteral Separator
```

## Semantics

- Binds a namespace prefix to a URI for use in the module's prolog and body.
- Multiple namespace declarations can appear in a module, but each prefix must be unique.
- Predefined prefixes (`xml`, `xs`, `xsi`, `fn`, `math`, `map`, `array`, `local`) are available without declaration.

## Examples

```xquery
declare namespace html = "http://www.w3.org/1999/xhtml";
declare namespace my = "http://example.com/my";

<html:div>{my:generate-content()}</html:div>
```

## Error Codes

- `XQST0033` — Duplicate namespace prefix in declarations
- `XQST0070` — Attempt to bind a reserved prefix (`xml`, `xmlns`)

## See Also

- [default-namespace](default-namespace.md)
- [module-declaration](module-declaration.md)

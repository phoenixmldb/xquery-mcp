---
name: option-declaration
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-option-declaration
---

# Option Declaration

Declares an option that provides implementation-defined or serialization settings.

## Syntax

```
OptionDecl ::= "declare" "option" EQName StringLiteral Separator
```

## Semantics

- Options are name-value pairs where the name is a QName and the value is a string literal.
- Options in the `output` namespace control serialization.
- Options in other namespaces are implementation-defined.
- Unknown options are silently ignored (no error is raised).

## Examples

```xquery
(: Serialization options :)
declare namespace output = "http://www.w3.org/2010/xslt-xquery-serialization";
declare option output:method "json";
declare option output:indent "yes";
declare option output:media-type "application/json";

(: Implementation-defined option :)
declare namespace opt = "http://example.com/options";
declare option opt:timeout "30";
```

## Error Codes

- `XPST0081` — Namespace prefix not declared

## See Also

- [serialization-declaration](serialization-declaration.md)

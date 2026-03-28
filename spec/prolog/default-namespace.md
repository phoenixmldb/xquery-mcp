---
name: default-namespace
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-default-namespace
---

# Default Namespace Declaration

Declares a default namespace for elements or functions, which applies when no prefix is used.

## Syntax

```
DefaultNamespaceDecl ::= "declare" "default" ("element" | "function")
                         "namespace" URILiteral Separator
```

## Semantics

- **`default element namespace`** — Applies to unprefixed element and type names in path expressions, constructors, and type declarations.
- **`default function namespace`** — Applies to unprefixed function names in function calls. The default is `http://www.w3.org/2005/xpath-functions` (the `fn` namespace).
- Each type (element/function) can have at most one default namespace declaration.

## Examples

```xquery
(: Default element namespace :)
declare default element namespace "http://www.w3.org/1999/xhtml";

<div><p>Hello</p></div>
(: Elements are in XHTML namespace :)

(: Default function namespace :)
declare default function namespace "http://example.com/my-functions";

(: Calls my-functions:process() instead of fn:process() :)
process($data)
```

## Error Codes

- `XQST0066` — More than one default element/function namespace declaration

## See Also

- [namespace-declaration](namespace-declaration.md)

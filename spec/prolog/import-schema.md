---
name: import-schema
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-schema-imports
---

# Import Schema

Imports an XML Schema, making its type definitions available for use in type declarations and validation.

## Syntax

```
SchemaImport ::= "import" "schema" SchemaPrefix? URILiteral
                 ("at" URILiteral ("," URILiteral)*)? Separator
SchemaPrefix ::= ("namespace" NCName "=") | ("default" "element" "namespace")
```

## Semantics

- Imports type definitions from an XML Schema.
- The imported types can be used in `instance of`, `cast as`, `treat as`, and type declarations.
- A namespace prefix can be bound to the schema's target namespace.
- The schema can be set as the default element namespace.
- Location hints are optional.
- Schema import is only available in schema-aware implementations.

## Examples

```xquery
(: Import schema with prefix :)
import schema namespace po = "http://example.com/purchase-order"
  at "po.xsd";

//po:order[po:total > 1000]

(: Import as default element namespace :)
import schema default element namespace "http://example.com/data";
```

## Error Codes

- `XQST0009` — Implementation does not support schema import
- `XQST0057` — Schema namespace is a zero-length string with a prefix
- `XQST0058` — Duplicate schema imports for the same namespace
- `XQST0059` — Schema not found

## See Also

- [import-module](import-module.md)
- [namespace-declaration](namespace-declaration.md)

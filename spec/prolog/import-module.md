---
name: import-module
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-module-imports
---

# Import Module

Imports a library module, making its public functions and variables available.

## Syntax

```
ModuleImport ::= "import" "module" ("namespace" NCName "=")? URILiteral
                 ("at" URILiteral ("," URILiteral)*)? Separator
```

## Semantics

- Imports all public functions and variables from the specified library module.
- The `namespace` clause binds a prefix to the module's namespace.
- The `at` clause provides location hints (URIs) where the module can be found.
- If no prefix is specified, the module's functions are accessible only via the full namespace URI.
- Circular module imports are forbidden.

## Examples

```xquery
(: Import with prefix :)
import module namespace math = "http://example.com/math";
math:square(5)

(: Import with location hint :)
import module namespace utils = "http://example.com/utils"
  at "lib/utils.xqm";
utils:process($data)

(: Import without prefix :)
import module "http://example.com/helpers";
```

## Error Codes

- `XQST0047` — Multiple imports of the same module namespace
- `XQST0059` — Module not found at the specified location
- `XQST0088` — Module namespace URI is a zero-length string
- `XQST0093` — Circular module import detected

## See Also

- [module-declaration](module-declaration.md)
- [import-schema](import-schema.md)

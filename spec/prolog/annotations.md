---
name: annotations
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-annotations
---

# Annotations

Annotations provide metadata for function and variable declarations, controlling visibility and other properties.

## Syntax

```
Annotation ::= "%" EQName ("(" Literal ("," Literal)* ")")?
```

## Semantics

Annotations appear before `function` or `variable` keywords in declarations. Standard annotations:

| Annotation | Applies to | Description |
|------------|-----------|-------------|
| `%public` | Functions, Variables | Visible outside the module (default for functions) |
| `%private` | Functions, Variables | Only visible within the declaring module |
| `%updating` | Functions | Function contains updating expressions (XQuery Update Facility) |

Additional annotations can be defined in implementation-specific namespaces (e.g., RESTXQ annotations for REST APIs).

Multiple annotations can be applied to the same declaration.

## Examples

```xquery
(: Public function (default) :)
declare %public function local:greet($name as xs:string) as xs:string {
  "Hello, " || $name
};

(: Private function :)
declare %private function local:helper() as xs:string {
  "internal"
};

(: Private variable :)
declare %private variable $state := 0;

(: Multiple annotations :)
declare %public %updating function local:update-doc($doc as node()) {
  replace node $doc/title with <title>Updated</title>
};

(: Implementation-specific annotation (RESTXQ) :)
declare namespace rest = "http://exquery.org/ns/restxq";
declare
  %rest:path("/api/users")
  %rest:GET
function local:get-users() {
  //user
};
```

## Error Codes

- `XQST0106` — Conflicting annotations (e.g., both `%public` and `%private`)
- `XQST0045` — Annotation in a reserved namespace

## See Also

- [function-declaration](function-declaration.md)
- [variable-declaration](variable-declaration.md)

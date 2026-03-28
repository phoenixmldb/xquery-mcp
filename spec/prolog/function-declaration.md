---
name: function-declaration
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-function-declarations
---

# Function Declaration

Declares a named function in the module prolog.

## Syntax

```
FunctionDecl ::= "declare" Annotation* "function" EQName "(" ParamList? ")"
                 ("as" SequenceType)?
                 (FunctionBody | "external")
                 Separator
FunctionBody ::= "{" Expr "}"
ParamList ::= Param ("," Param)*
Param ::= "$" EQName TypeDeclaration?
```

## Semantics

- Declares a function that can be called from anywhere in the module (or from other modules if `%public`).
- Functions in a main module are in the `local` namespace by default.
- Functions in a library module must be in the module's target namespace.
- Functions can be annotated with `%public` (default) or `%private`.
- External functions have their implementation provided by the environment.
- Functions can be recursive. XQuery supports tail-call optimization in some implementations.

## Examples

```xquery
(: Simple function :)
declare function local:greet($name as xs:string) as xs:string {
  "Hello, " || $name || "!"
};

(: Function with multiple parameters :)
declare function local:add($a as xs:integer, $b as xs:integer) as xs:integer {
  $a + $b
};

(: Recursive function :)
declare function local:factorial($n as xs:integer) as xs:integer {
  if ($n <= 1) then 1
  else $n * local:factorial($n - 1)
};

(: Private function :)
declare %private function local:helper($input as item()*) as item()* {
  $input
};

(: Function with no return type :)
declare function local:process($doc as document-node()) {
  $doc//item[price > 100]
};
```

## Error Codes

- `XQST0034` — Duplicate function declaration (same name and arity)
- `XQST0045` — Function name is in a reserved namespace
- `XPTY0004` — Return value does not match declared type

## See Also

- [variable-declaration](variable-declaration.md)
- [annotations](annotations.md)
- [module-declaration](module-declaration.md)

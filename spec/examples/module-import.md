---
name: module-import
category: example
since: "1.0"
spec_url: https://www.w3.org/TR/xquery-31/#id-module-import
---

# Module declaration and import-module syntax

## Library module (util.xqm)

```xquery
(:
  Library module — declares a namespace, defines functions and variables.
  File: util.xqm  (any extension works; .xqm is conventional)
:)
module namespace util = "http://example.com/util";

(: Private helper — not callable from outside :)
declare %private function util:trim($s as xs:string) as xs:string {
  replace($s, "^\s+|\s+$", "")
};

(: Public function — importable by other modules :)
declare function util:normalize($s as xs:string) as xs:string {
  lower-case(util:trim($s))
};

(: Exported variable :)
declare variable $util:version as xs:string := "1.0.0";
```

## Main query (main.xq) — importing the library

```xquery
xquery version "3.1";

(:
  Import the library module using its namespace URI.
  "at" hints the processor to a file location; without "at" the
  processor uses its own module catalog.
:)
import module namespace util = "http://example.com/util"
  at "util.xqm";

(: Use the imported function and variable :)
let $raw := ("  Hello ", " WORLD  ")
return (
  $util:version,
  for $s in $raw
  return util:normalize($s)
)
(: Result: "1.0.0", "hello", "world" :)
```

## Module with constructor functions and type imports

```xquery
xquery version "3.1";
import module namespace math = "http://www.w3.org/2005/xpath-functions/math";

(: Built-in math module — no "at" needed for standard modules :)
let $pi := math:pi()
return format-number($pi, "0.0000")
```

## Main module declaration

```xquery
(:
  A main module optionally declares its own namespace for self-referential use.
  Unlike library modules, the module namespace is not re-exported.
:)
module namespace app = "http://example.com/app";

import module namespace util = "http://example.com/util"
  at "util.xqm";

declare function app:greet($name as xs:string) as xs:string {
  concat("Hello, ", util:normalize($name), "!")
};
```

## What it does

A **library module** begins with `module namespace prefix = "uri";` and
exports every `declare function` and `declare variable` that is not marked
`%private`. A **main module** begins with `xquery version "3.1";` (no
`module namespace` declaration) and is the entry point for execution.

`import module namespace prefix = "uri" at "location.xqm"` pulls a library
module into scope. The `at` clause is a **hint** — processors may ignore it
if they have the module registered in a catalog. Multiple `at` locations
can be given as a sequence.

## Common pitfalls

- The module namespace URI must match **exactly** between the `module namespace`
  declaration in the library and the `import module namespace` in the consumer —
  a typo causes `XQST0059`.
- `%private` functions are not callable from importing modules; `%public` is
  the default when no annotation is given.
- Circular imports are allowed in XQuery 3.x but may confuse some processors.
  Break cycles by extracting shared utilities into a third module.
- The `at` hint is processor-dependent; for portable code, register modules
  in the processor's catalog rather than relying on relative file paths.

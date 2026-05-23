---
name: try-catch
category: example
since: "3.0"
spec_url: https://www.w3.org/TR/xquery-31/#id-try-catch
---

# try-catch with $err:code and structured error handling

## Basic try-catch

```xquery
xquery version "3.1";
declare namespace err = "http://www.w3.org/2005/xqt-errors";

let $input := "not-a-number"
return
  try {
    xs:integer($input)
  } catch err:FORG0001 {
    (: Cast failed — return a default :)
    -1
  }
```

## Catch all errors and surface details

```xquery
xquery version "3.1";
declare namespace err = "http://www.w3.org/2005/xqt-errors";

declare function local:safe-doc($uri as xs:string) as document-node()? {
  try {
    doc($uri)
  } catch * {
    (: $err:code    — xs:QName of the error
       $err:description — human-readable message
       $err:value   — error object if provided
       $err:line-number / $err:column-number — source location :)
    error((), concat("Failed to load ", $uri, ": ",
                     local-name-from-QName($err:code),
                     " — ", $err:description))
  }
};

local:safe-doc('data.xml')
```

## Re-throwing a subset of errors

```xquery
xquery version "3.1";
declare namespace err = "http://www.w3.org/2005/xqt-errors";

try {
  doc('maybe-missing.xml')//item[@id = '42']
} catch err:FODC0002 {
  (: Document not found — return empty sequence :)
  ()
} catch * {
  (: Anything else — re-raise :)
  error($err:code, $err:description, $err:value)
}
```

## What it does

`try { expr } catch pattern { handler }` evaluates `expr`; if it raises a
dynamic error whose error code QName matches `pattern`, the handler
expression is evaluated instead. The catch clause has access to four
implicit variables from the `err:` namespace:

- `$err:code` — `xs:QName` of the error
- `$err:description` — string description
- `$err:value` — arbitrary error value (may be absent)
- `$err:line-number`, `$err:column-number`, `$err:module` — source location

Use `catch *` to match any error code, or list specific codes separated by
`|` to catch multiple.

## Common pitfalls

- The `err:` namespace prefix must be declared — it is not predeclared in
  XQuery 3.x despite the implicit variables.
- `catch *` catches **all** errors including `XPST0003` (parse errors in
  dynamically compiled expressions) — be specific when possible.
- Errors raised inside a `catch` block propagate normally — you must nest
  another `try` to catch them.
- Static errors (e.g., undefined variable) are not catchable at runtime;
  they are reported before execution begins.

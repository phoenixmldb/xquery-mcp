---
name: static-context
category: concept
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-xq-static-context
---

# Static Context

The static context contains information available during static analysis (compilation) of an XQuery expression, before the query is evaluated.

## Components

| Component | Description |
|-----------|-------------|
| **Statically known namespaces** | Namespace bindings from prolog declarations and predefined prefixes |
| **Default element/type namespace** | Namespace for unprefixed element and type names |
| **Default function namespace** | Namespace for unprefixed function names (default: `fn`) |
| **In-scope schema types** | Type definitions available from schema imports |
| **In-scope variables** | Variables declared in prolog or bound by expressions |
| **In-scope functions** | Functions available (built-in + declared + imported) |
| **Statically known collations** | Collation URIs recognized by the implementation |
| **Default collation** | Collation used when none is specified |
| **Construction mode** | `strip` or `preserve` — affects type annotations on constructed nodes |
| **Ordering mode** | `ordered` or `unordered` — default ordering of result sequences |
| **Default order for empty sequences** | `greatest` or `least` — how empty sequences sort |
| **Boundary-space policy** | `strip` or `preserve` — handling of whitespace in constructors |
| **Copy-namespaces mode** | `preserve`/`no-preserve` and `inherit`/`no-inherit` |
| **Base URI** | Base URI for resolving relative URIs |
| **Statically known documents** | Documents known to be available |
| **Statically known collections** | Collections known to be available |

## Prolog Declarations Affecting Static Context

```xquery
declare default element namespace "http://example.com";
declare default function namespace "http://example.com/fn";
declare default collation "http://www.w3.org/2013/collation/UCA";
declare default order empty least;
declare construction strip;
declare ordering ordered;
declare boundary-space preserve;
declare copy-namespaces preserve, inherit;
declare base-uri "http://example.com/";
```

## See Also

- [dynamic-context](dynamic-context.md)
- [focus](focus.md)

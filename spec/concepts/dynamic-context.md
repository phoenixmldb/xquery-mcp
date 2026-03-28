---
name: dynamic-context
category: concept
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-xq-dynamic-context
---

# Dynamic Context

The dynamic context contains information available during evaluation (runtime) of an XQuery expression. It includes the static context plus runtime-specific values.

## Components

| Component | Description |
|-----------|-------------|
| **Focus** | Context item, context position, and context size |
| **Variable values** | Runtime values of all in-scope variables |
| **Current dateTime** | The current date, time, and timezone (stable within a query) |
| **Implicit timezone** | Timezone used when comparing timezone-less values |
| **Available documents** | Documents available via `fn:doc()` |
| **Available collections** | Collections available via `fn:collection()` |
| **Default collection** | The collection returned by `fn:collection()` with no argument |
| **Available URI collections** | URI collections available via `fn:uri-collection()` |
| **Available environment variables** | Environment variables available via `fn:environment-variable()` |

## Semantics

The dynamic context is an extension of the static context. Everything in the static context is also available at runtime, plus the additional components listed above.

The **current dateTime** is set once at the start of query evaluation and remains constant throughout. This ensures that multiple calls to `fn:current-dateTime()` within the same query return identical values.

External variables are resolved in the dynamic context — their values are provided by the calling application.

## Examples

```xquery
(: Accessing dynamic context components :)
fn:current-dateTime()        (: current dateTime :)
fn:implicit-timezone()       (: implicit timezone :)
fn:doc("data.xml")           (: available documents :)
fn:collection("my-collection") (: available collections :)
fn:environment-variable("HOME") (: environment variables :)
```

## See Also

- [static-context](static-context.md)
- [focus](focus.md)

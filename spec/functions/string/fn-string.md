---
name: fn-string
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-string
---

# fn:string

Returns the string value of the argument.

## Signature

`fn:string() as xs:string`
`fn:string($arg as item()?) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()?` | The item whose string value is returned. If omitted, uses the context item. |

## Semantics

- If `$arg` is absent (empty sequence), returns the zero-length string.
- If `$arg` is a node, returns the string value of the node.
- If `$arg` is an atomic value, returns the result of casting it to `xs:string`.
- If `$arg` is a function item (including maps and arrays), a type error is raised.
- If called with no argument, uses the context item (`.`).

## Examples

```xquery
fn:string(42)
(: Result: "42" :)

fn:string(true())
(: Result: "true" :)

fn:string(())
(: Result: "" :)

fn:string(<name>Alice</name>)
(: Result: "Alice" :)

(: Using context item :)
//book/title/fn:string()
```

## Error Codes

- `XPDY0002` — Context item is absent when called with no argument
- `FOTY0014` — Argument is a function item (map, array, or function)

## See Also

- [fn-concat](fn-concat.md)
- [fn-string-join](fn-string-join.md)

---
name: fn-not
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-not
---

# fn:not

Returns the boolean negation of the effective boolean value of the argument.

## Signature

`fn:not($arg as item()*) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()*` | The value to negate |

## Semantics

- Computes `fn:boolean($arg)` and returns its negation.
- `fn:not(true())` returns `false`; `fn:not(false())` returns `true`.
- `fn:not(())` returns `true` (empty sequence has EBV `false`).

## Examples

```xquery
fn:not(true())
(: Result: false :)

fn:not(false())
(: Result: true :)

fn:not(0)
(: Result: true :)

fn:not(())
(: Result: true :)

fn:not(//book)
(: Result: true if no book elements exist :)
```

## Error Codes

- `FORG0006` — Same as `fn:boolean`

## See Also

- [fn-boolean](fn-boolean.md)

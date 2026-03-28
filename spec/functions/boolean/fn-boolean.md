---
name: fn-boolean
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-boolean
---

# fn:boolean

Computes the effective boolean value of the argument.

## Signature

`fn:boolean($arg as item()*) as xs:boolean`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$arg` | `item()*` | The value to convert to boolean |

## Semantics

The effective boolean value (EBV) rules:

1. If `$arg` is the empty sequence, returns `false`.
2. If `$arg` is a sequence starting with a node, returns `true`.
3. If `$arg` is a single `xs:boolean`, returns that value.
4. If `$arg` is a single `xs:string` or `xs:untypedAtomic`, returns `false` if zero-length, `true` otherwise.
5. If `$arg` is a single numeric value, returns `false` if zero or `NaN`, `true` otherwise.
6. Otherwise, raises `FORG0006`.

## Examples

```xquery
fn:boolean(true())
(: Result: true :)

fn:boolean(0)
(: Result: false :)

fn:boolean(42)
(: Result: true :)

fn:boolean("")
(: Result: false :)

fn:boolean("hello")
(: Result: true :)

fn:boolean(())
(: Result: false :)

fn:boolean(<node/>)
(: Result: true :)
```

## Error Codes

- `FORG0006` — `$arg` is a sequence of more than one item starting with an atomic value, or a single function item

## See Also

- [fn-not](fn-not.md)
- [fn-true](fn-true.md)
- [fn-false](fn-false.md)
- [effective-boolean-value](../../concepts/effective-boolean-value.md)

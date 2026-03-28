---
name: fn-true
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-true
---

# fn:true

Returns the boolean value `true`.

## Signature

`fn:true() as xs:boolean`

## Semantics

Returns `xs:boolean` value `true`. This function takes no arguments.

## Examples

```xquery
fn:true()
(: Result: true :)

if (fn:true()) then "yes" else "no"
(: Result: "yes" :)
```

## See Also

- [fn-false](fn-false.md)
- [fn-boolean](fn-boolean.md)

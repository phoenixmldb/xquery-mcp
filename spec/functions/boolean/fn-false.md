---
name: fn-false
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-false
---

# fn:false

Returns the boolean value `false`.

## Signature

`fn:false() as xs:boolean`

## Semantics

Returns `xs:boolean` value `false`. This function takes no arguments.

## Examples

```xquery
fn:false()
(: Result: false :)

if (fn:false()) then "yes" else "no"
(: Result: "no" :)
```

## See Also

- [fn-true](fn-true.md)
- [fn-boolean](fn-boolean.md)

---
name: fn-xml-to-json
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-xml-to-json
---

# fn:xml-to-json

Converts an XML representation of JSON (as produced by `fn:json-to-xml`) back to a JSON string.

## Signature

`fn:xml-to-json($input as node()?) as xs:string?`
`fn:xml-to-json($input as node()?, $options as map(*)) as xs:string?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$input` | `node()?` | XML document in the JSON XML representation |
| `$options` | `map(*)` | Options: `indent` (xs:boolean) for pretty-printing |

## Semantics

- Converts XML in the `http://www.w3.org/2005/xpath-functions` namespace back to JSON.
- The inverse of `fn:json-to-xml`.
- If `$input` is the empty sequence, returns the empty sequence.
- The `indent` option controls whether the output is pretty-printed.

## Examples

```xquery
let $xml :=
  <map xmlns="http://www.w3.org/2005/xpath-functions">
    <string key="name">Alice</string>
    <number key="age">30</number>
  </map>
return fn:xml-to-json($xml)
(: Result: '{"name":"Alice","age":30}' :)

fn:xml-to-json($xml, map{"indent": true()})
```

## Error Codes

- `FOJS0006` — Invalid XML structure for JSON conversion

## See Also

- [fn-json-to-xml](fn-json-to-xml.md)
- [fn-parse-json](fn-parse-json.md)

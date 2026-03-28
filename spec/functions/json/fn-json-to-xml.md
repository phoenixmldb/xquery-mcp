---
name: fn-json-to-xml
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-json-to-xml
---

# fn:json-to-xml

Converts a JSON string to an XML representation.

## Signature

`fn:json-to-xml($json-text as xs:string?) as document-node()?`
`fn:json-to-xml($json-text as xs:string?, $options as map(*)) as document-node()?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$json-text` | `xs:string?` | The JSON string |
| `$options` | `map(*)` | Options map (same as `fn:parse-json`) |

## Semantics

Converts JSON to an XML representation in the namespace `http://www.w3.org/2005/xpath-functions`. The mapping is:

| JSON | XML element |
|------|-------------|
| Object | `<map>` with `<string>`, `<number>`, etc. children with `@key` attributes |
| Array | `<array>` |
| String | `<string>` |
| Number | `<number>` |
| Boolean | `<boolean>` |
| Null | `<null>` |

## Examples

```xquery
fn:json-to-xml('{"name": "Alice", "age": 30}')
(: Result:
<map xmlns="http://www.w3.org/2005/xpath-functions">
  <string key="name">Alice</string>
  <number key="age">30</number>
</map>
:)

fn:json-to-xml('[1, "two", true, null]')
(: Result:
<array xmlns="http://www.w3.org/2005/xpath-functions">
  <number>1</number>
  <string>two</string>
  <boolean>true</boolean>
  <null/>
</array>
:)
```

## Error Codes

- `FOJS0001` — Invalid JSON syntax

## See Also

- [fn-xml-to-json](fn-xml-to-json.md)
- [fn-parse-json](fn-parse-json.md)

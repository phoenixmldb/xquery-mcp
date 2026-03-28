---
name: fn-json-doc
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-json-doc
---

# fn:json-doc

Reads a JSON document from a URI and parses it.

## Signature

`fn:json-doc($href as xs:string?) as item()?`
`fn:json-doc($href as xs:string?, $options as map(*)) as item()?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$href` | `xs:string?` | URI of the JSON document |
| `$options` | `map(*)` | Options map (same as `fn:parse-json`) |

## Semantics

- Retrieves the resource at `$href` as a string, then parses it as JSON.
- Equivalent to `fn:parse-json(fn:unparsed-text($href), $options)`.
- If `$href` is the empty sequence, returns the empty sequence.
- The result is cached: multiple calls with the same URI return the same result.

## Examples

```xquery
fn:json-doc("data.json")

fn:json-doc("https://api.example.com/data.json")

let $data := fn:json-doc("config.json")
return $data?settings?timeout
```

## Error Codes

- `FOJS0001` — Invalid JSON syntax
- `FOUT1170` — Cannot retrieve the resource
- `FOUT1190` — Cannot decode the resource

## See Also

- [fn-parse-json](fn-parse-json.md)
- [fn-json-to-xml](fn-json-to-xml.md)

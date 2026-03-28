---
name: fn-parse-json
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-parse-json
---

# fn:parse-json

Parses a JSON string and returns the corresponding XQuery value (maps, arrays, strings, numbers, booleans, or null).

## Signature

`fn:parse-json($json-text as xs:string?) as item()?`
`fn:parse-json($json-text as xs:string?, $options as map(*)) as item()?`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$json-text` | `xs:string?` | The JSON string to parse |
| `$options` | `map(*)` | Options map with keys: `liberal`, `duplicates`, `escape`, `fallback` |

## Semantics

- Parses `$json-text` as JSON and converts to XQuery types:
  - JSON objects become `map(*)` values
  - JSON arrays become `array(*)` values
  - JSON strings become `xs:string`
  - JSON numbers become `xs:double`
  - JSON `true`/`false` become `xs:boolean`
  - JSON `null` becomes the empty sequence
- If `$json-text` is the empty sequence, returns the empty sequence.
- Options:
  - `duplicates`: `"reject"` (error on duplicate keys), `"use-first"`, `"use-last"` (default)
  - `liberal`: `true` to accept non-standard JSON
  - `escape`: `true` to escape special characters
  - `fallback`: function for unrecognized escape sequences

## Examples

```xquery
fn:parse-json('{"name": "Alice", "age": 30}')
(: Result: map{"name": "Alice", "age": 30.0e0} :)

fn:parse-json('[1, 2, 3]')
(: Result: [1.0e0, 2.0e0, 3.0e0] :)

fn:parse-json('"hello"')
(: Result: "hello" :)

fn:parse-json('null')
(: Result: () :)

fn:parse-json('{"a": 1}', map{"duplicates": "reject"})
```

## Error Codes

- `FOJS0001` — Invalid JSON syntax
- `FOJS0003` — Duplicate key when `duplicates` option is `"reject"`

## See Also

- [fn-json-doc](fn-json-doc.md)
- [fn-json-to-xml](fn-json-to-xml.md)

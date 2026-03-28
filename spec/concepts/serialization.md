---
name: serialization
category: concept
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-serialization
---

# Serialization

Serialization is the process of converting the result of an XQuery expression (the XDM data model) into a sequence of bytes or characters for output.

## Serialization Methods

| Method | Output Format |
|--------|--------------|
| `xml` | Well-formed XML |
| `html` | HTML (4.0 or 5.0) |
| `xhtml` | XHTML |
| `text` | Plain text (string values of all items) |
| `json` | JSON |
| `adaptive` | Mixed output supporting all XDM types (default in many implementations) |

## Serialization Parameters

Parameters are set via `declare option output:` in the prolog or via the calling API.

| Parameter | Description |
|-----------|-------------|
| `method` | Serialization method |
| `encoding` | Character encoding (e.g., `UTF-8`) |
| `indent` | `yes`/`no` — whether to indent output |
| `omit-xml-declaration` | `yes`/`no` |
| `media-type` | MIME type |
| `version` | Output version (e.g., `1.0` for XML) |
| `standalone` | `yes`/`no`/`omit` |
| `cdata-section-elements` | Elements to output as CDATA |
| `suppress-indentation` | Elements where indentation is suppressed |
| `item-separator` | Separator between top-level items |
| `byte-order-mark` | `yes`/`no` |

## Adaptive Serialization

The `adaptive` method can serialize any XDM value:
- Atomic values are output as their string representation
- Nodes are serialized as XML
- Maps are serialized as `map{"key":value,...}`
- Arrays are serialized as `[member1,member2,...]`
- Function items are output as `(anonymous-function)#arity` or similar

## Examples

```xquery
(: JSON serialization :)
declare namespace output = "http://www.w3.org/2010/xslt-xquery-serialization";
declare option output:method "json";
declare option output:indent "yes";

map {
  "name": "Alice",
  "scores": [95, 87, 92]
}

(: XML serialization with indent :)
declare option output:method "xml";
declare option output:indent "yes";
declare option output:omit-xml-declaration "yes";

<root>
  <item>value</item>
</root>
```

## Error Codes

- `SEPM0016` — Invalid serialization parameter value
- `SERE0003` — Serialization of a value that requires a character not representable in the chosen encoding
- `SESU0007` — Unsupported encoding
- `SEPM0009` — The `omit-xml-declaration` parameter has the value `yes`, but standalone is `yes`

## See Also

- [serialization-declaration](../prolog/serialization-declaration.md)

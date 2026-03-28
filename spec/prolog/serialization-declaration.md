---
name: serialization-declaration
category: prolog
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-serialization
---

# Serialization Declaration

Controls how the query result is serialized to text output using output declarations in the prolog.

## Syntax

```
OutputDecl ::= "declare" "option" "output:" NCName StringLiteral Separator
```

Requires the `output` namespace to be bound to `http://www.w3.org/2010/xslt-xquery-serialization`.

## Semantics

Serialization parameters control the conversion of the XQuery result tree to bytes or characters. Common parameters:

| Parameter | Values | Description |
|-----------|--------|-------------|
| `method` | `xml`, `html`, `xhtml`, `text`, `json`, `adaptive` | Serialization method |
| `indent` | `yes`, `no` | Whether to indent output |
| `omit-xml-declaration` | `yes`, `no` | Whether to omit the XML declaration |
| `encoding` | e.g., `UTF-8` | Character encoding |
| `media-type` | e.g., `application/xml` | MIME type |
| `version` | e.g., `1.0` | XML version |
| `standalone` | `yes`, `no`, `omit` | Standalone declaration |
| `cdata-section-elements` | QName list | Elements whose content is serialized as CDATA |
| `json-node-output-method` | `xml`, `html`, `xhtml`, `text` | Method for XML nodes within JSON serialization |

## Examples

```xquery
declare namespace output = "http://www.w3.org/2010/xslt-xquery-serialization";

(: Serialize as indented XML :)
declare option output:method "xml";
declare option output:indent "yes";
declare option output:omit-xml-declaration "yes";

(: Serialize as JSON :)
declare option output:method "json";
declare option output:indent "yes";

(: Serialize as HTML5 :)
declare option output:method "html";
declare option output:version "5.0";

(: Serialize as plain text :)
declare option output:method "text";
declare option output:encoding "UTF-8";
```

## Error Codes

- `XQST0108` — Output declaration in a library module
- `XQST0110` — Duplicate serialization parameter
- `SEPM0016` — Invalid serialization parameter value

## See Also

- [option-declaration](option-declaration.md)

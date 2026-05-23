---
name: fn:transform
category: example
since: "3.0"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-transform
---

# Calling an XSLT stylesheet from XQuery

## Basic call

```xquery
xquery version "3.1";
import module namespace map = "http://www.w3.org/2005/xpath-functions/map";

(: Apply an XSLT 3.0 stylesheet to a source document :)
let $source := doc('data.xml')
let $result := fn:transform(map{
  "stylesheet-location" : "render.xsl",
  "source-node"         : $source,
  "delivery-format"     : "document"
})
(: Principal output document is always at key "" :)
return map:get($result, "")
```

## Passing parameters to the stylesheet

```xquery
xquery version "3.1";
import module namespace map = "http://www.w3.org/2005/xpath-functions/map";

let $result := fn:transform(map{
  "stylesheet-location" : "report.xsl",
  "source-node"         : doc('data.xml'),
  "stylesheet-params"   : map{
    fn:QName("", "title")     : "Quarterly Report",
    fn:QName("", "year")      : xs:integer(2025),
    fn:QName("", "debug")     : false()
  },
  "delivery-format"     : "document"
})
return map:get($result, "")
```

## Capturing multiple result documents

```xquery
xquery version "3.1";
import module namespace map = "http://www.w3.org/2005/xpath-functions/map";

let $result := fn:transform(map{
  "stylesheet-location" : "split-output.xsl",
  "source-node"         : doc('data.xml'),
  "delivery-format"     : "document"
})
(: Iterate over all result documents produced by xsl:result-document :)
for $uri in map:keys($result)
return
  <output uri="{$uri}" size="{string-length(serialize(map:get($result, $uri)))}"/>
```

## What it does

`fn:transform` invokes an XSLT transformation from within an XQuery
expression. The argument is an options map. Commonly used keys:

- `"stylesheet-location"` — URI string of the stylesheet
- `"stylesheet-node"` — an already-parsed stylesheet document node (avoids re-parsing)
- `"source-node"` — the principal source document node
- `"stylesheet-params"` — map of `xs:QName → item()*` for top-level params
- `"delivery-format"` — `"document"` returns a map of result documents;
  `"serialized"` returns a serialized string; `"raw"` returns the raw sequence

The return value with `"delivery-format": "document"` is a map from result
URI strings to document nodes. The principal output is at key `""` (empty
string).

## Common pitfalls

- The return value is a **map**, not a node. Always use `map:get($result, "")`
  or `$result?""` to get the principal output document.
- `"stylesheet-params"` keys must be `xs:QName` values — use `fn:QName(namespace, local)`.
  Passing plain strings as keys causes a type error.
- With `"delivery-format": "raw"`, returned nodes may be anchored in an
  inner document that does not survive the call across all implementations.
  Prefer `"document"` for pipeline scenarios.
- The stylesheet URI is resolved relative to the **base URI of the XQuery
  module**, not the current working directory.

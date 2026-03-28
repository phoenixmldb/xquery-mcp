---
name: computed-constructors
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-computedConstructors
---

# Computed Constructors

Computed constructors create XML nodes using keywords and expressions for both the name and content, allowing fully dynamic node construction.

## Syntax

```
CompDocConstructor     ::= "document" "{" Expr "}"
CompElemConstructor    ::= "element" (EQName | ("{" Expr "}")) "{" ContentExpr? "}"
CompAttrConstructor    ::= "attribute" (EQName | ("{" Expr "}")) "{" Expr? "}"
CompTextConstructor    ::= "text" "{" Expr "}"
CompCommentConstructor ::= "comment" "{" Expr "}"
CompPIConstructor      ::= "processing-instruction"
                           (NCName | ("{" Expr "}")) "{" Expr? "}"
CompNamespaceConstructor ::= "namespace" (Prefix | ("{" Expr "}")) "{" Expr "}"
```

## Semantics

Computed constructors use keywords (`element`, `attribute`, `text`, `comment`, `processing-instruction`, `document`, `namespace`) followed by a name expression and a content expression, both enclosed in braces.

- The **name** can be a literal QName or a computed expression in braces.
- The **content** expression is evaluated and its result becomes the content of the new node.
- For elements, the content can include attributes, child elements, and text.
- For text nodes, adjacent text is merged into a single text node. If the content is empty, no text node is created.

## Examples

```xquery
(: Computed element with literal name :)
element book { "XQuery Fundamentals" }

(: Computed element with dynamic name :)
element { $tag-name } { $content }

(: Computed attribute :)
element product {
  attribute id { $product-id },
  attribute name { $product-name },
  text { $description }
}

(: Computed text node :)
text { "Hello, World!" }

(: Computed comment :)
comment { "This is a generated comment" }

(: Computed processing instruction :)
processing-instruction xml-stylesheet { 'type="text/xsl" href="style.xsl"' }

(: Computed document node :)
document {
  element root {
    for $item in $items
    return element item { $item }
  }
}

(: Computed namespace :)
element foo {
  namespace ns { "http://example.com/ns" },
  attribute ns:bar { "value" }
}

(: Dynamic element and attribute names :)
for $col in ("name", "age", "city")
return element { $col } { $data/@*[local-name() = $col]/string() }
```

## Error Codes

- `XQDY0074` — Computed element/attribute name cannot be cast to `xs:QName`
- `XQDY0025` — Duplicate attribute names in a computed element constructor
- `XQDY0041` — Processing instruction target is "xml" (case-insensitive)
- `XQDY0044` — Computed attribute node has the name `xmlns`
- `XQDY0064` — Processing instruction target contains ":"
- `XPTY0004` — Type error in name or content expression

## See Also

- [constructors](constructors.md)

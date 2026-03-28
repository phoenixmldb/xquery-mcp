---
name: constructors
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-constructors
---

# Direct Element Constructors

Direct constructors create XML nodes using XML-like literal syntax within XQuery expressions.

## Syntax

```
DirElemConstructor ::= "<" QName DirAttributeList
                       ("/>" | (">" DirElemContent* "</" QName S? ">"))
DirAttributeList ::= (S (QName S? "=" S? DirAttributeValue)*)?
DirAttributeValue ::= ('"' (EscapeQuot | QuotAttrValueContent)* '"')
                     | ("'" (EscapeApos | AposAttrValueContent)* "'")
DirElemContent ::= DirectConstructor
                 | CDataSection
                 | CommonContent
                 | ElementContentChar
```

## Semantics

Direct element constructors create new element nodes. The constructor copies the appearance of XML literal syntax. Enclosed expressions within `{` `}` are evaluated and their results are inserted into the content.

- **Attributes** are specified as `name="value"` or `name="{expr}"` within the start tag.
- **Content** can include literal text, nested elements, enclosed expressions `{expr}`, and CDATA sections.
- **Namespace declarations** can appear as attributes: `xmlns:prefix="uri"` or `xmlns="uri"`.

Each direct constructor creates a **new node** with a new identity, regardless of whether identical content was constructed previously.

## Examples

```xquery
(: Simple element :)
<greeting>Hello, World!</greeting>

(: Element with attributes :)
<book isbn="978-0-123456-78-9" lang="en">
  <title>XQuery in Action</title>
</book>

(: Enclosed expressions in content :)
<result>{2 + 3}</result>
(: Result: <result>5</result> :)

(: Enclosed expressions in attributes :)
<item count="{count(//book)}"/>

(: Mixed content :)
<p>There are {count(//book)} books by {$author}.</p>

(: Nested constructors :)
<library>
{
  for $book in //book
  return
    <entry>
      <title>{$book/title/string()}</title>
      <author>{$book/author/string()}</author>
    </entry>
}
</library>

(: Namespace declarations :)
<html xmlns="http://www.w3.org/1999/xhtml">
  <body><p>Hello</p></body>
</html>
```

## Error Codes

- `XQST0022` — Namespace value in a direct constructor is not a literal
- `XQST0070` — Attempt to redefine a reserved namespace prefix
- `XQST0085` — Namespace URI is a zero-length string with a non-empty prefix

## See Also

- [computed-constructors](computed-constructors.md)
- [string-constructors](string-constructors.md)

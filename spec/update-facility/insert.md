---
name: insert
category: update
since: "3.0"
spec_url: https://www.w3.org/TR/xquery-update-30/#id-insert
---

# Insert Expression

Inserts one or more nodes into or adjacent to a target node.

## Syntax

```
InsertExpr ::= "insert" ("node" | "nodes") SourceExpr InsertExprTargetChoice TargetExpr
InsertExprTargetChoice ::= (("as" ("first" | "last"))? "into")
                         | "after"
                         | "before"
```

## Semantics

- **`into`** — Inserts nodes as children of the target element or document node. Without `as first`/`as last`, the position is implementation-defined.
- **`as first into`** — Inserts as the first child(ren).
- **`as last into`** — Inserts as the last child(ren).
- **`before`** — Inserts immediately before the target node.
- **`after`** — Inserts immediately after the target node.

The source nodes are copied before insertion. The original nodes are unchanged.

## Examples

```xquery
(: Insert as last child :)
insert node <author>Smith</author> as last into /book

(: Insert before a node :)
insert node <preface>...</preface> before /book/chapter[1]

(: Insert after a node :)
insert node <appendix>...</appendix> after /book/chapter[last()]

(: Insert multiple nodes :)
insert nodes (
  <item>A</item>,
  <item>B</item>
) into /list

(: Insert attribute :)
insert node attribute lang { "en" } into /document

(: Insert as first child :)
insert node <header/> as first into /page
```

## Error Codes

- `XUDY0005` — Target of `into` is not an element or document node
- `XUDY0006` — Target of `before`/`after` has no parent
- `XUDY0009` — Target for attribute insertion is not an element
- `XUTY0004` — Inserting an attribute using `before`/`after` a non-attribute node
- `XUTY0005` — Source for `into` contains a non-attribute mixed with attributes

## See Also

- [delete](delete.md)
- [replace](replace.md)
- [pending-update-list](pending-update-list.md)

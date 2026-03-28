---
name: rename
category: update
since: "3.0"
spec_url: https://www.w3.org/TR/xquery-update-30/#id-rename
---

# Rename Expression

Changes the name (QName) of an element, attribute, or processing instruction node.

## Syntax

```
RenameExpr ::= "rename" "node" TargetExpr "as" NewNameExpr
```

## Semantics

- Changes the name of the target node without affecting its content, children, or attributes.
- The target must be a single element, attribute, or processing instruction node.
- The new name expression is evaluated and cast to `xs:QName`.
- The node's identity is preserved.

## Examples

```xquery
(: Rename element :)
rename node /book/title as "heading"

(: Rename attribute :)
rename node /book/@isbn as "id"

(: Rename with computed name :)
rename node $node as QName("http://example.com/ns", "new-name")

(: Rename processing instruction :)
rename node //processing-instruction(old-target) as "new-target"
```

## Error Codes

- `XUTY0012` — Target is not an element, attribute, or processing instruction
- `XUDY0015` — Renaming would create a conflicting namespace binding
- `XQDY0074` — New name cannot be cast to `xs:QName`

## See Also

- [replace](replace.md)
- [insert](insert.md)

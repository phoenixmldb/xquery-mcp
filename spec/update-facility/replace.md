---
name: replace
category: update
since: "3.0"
spec_url: https://www.w3.org/TR/xquery-update-30/#id-replace
---

# Replace Expression

Replaces a node or the value of a node.

## Syntax

```
ReplaceExpr ::= "replace" ("value" "of")? "node" TargetExpr "with" ExprSingle
```

## Semantics

- **`replace node`** — Replaces the target node entirely with the replacement node(s). The target must have a parent.
- **`replace value of node`** — Replaces the content/value of the target node while preserving the node identity, name, and type.

For `replace value of node`:
- For element nodes: replaces all children with a single text node.
- For attribute, text, comment, and PI nodes: replaces the string value.

## Examples

```xquery
(: Replace entire node :)
replace node /book/title
with <title>New Title</title>

(: Replace value only :)
replace value of node /book/title
with "New Title"

(: Replace attribute value :)
replace value of node /book/@isbn
with "978-0-000000-00-0"

(: Replace with computed content :)
replace node /report/summary
with <summary>{
  concat("Report as of ", current-dateTime())
}</summary>
```

## Error Codes

- `XUDY0008` — Target has no parent (for `replace node`)
- `XUDY0009` — Target of `replace value of node` for attributes is not an element
- `XUTY0006` — Target is not a single element, attribute, text, comment, or PI node
- `XUTY0008` — Replacement for an element contains an attribute after a non-attribute

## See Also

- [rename](rename.md)
- [insert](insert.md)
- [delete](delete.md)

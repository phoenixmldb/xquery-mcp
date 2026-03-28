---
name: delete
category: update
since: "3.0"
spec_url: https://www.w3.org/TR/xquery-update-30/#id-delete
---

# Delete Expression

Removes one or more nodes from their parent.

## Syntax

```
DeleteExpr ::= "delete" ("node" | "nodes") TargetExpr
```

## Semantics

- Removes each target node from its parent.
- If a target node has no parent (e.g., it is a root node), it is silently ignored.
- The target expression must return a sequence of nodes.
- Deleted nodes become disconnected from the tree but may still be referenced.

## Examples

```xquery
(: Delete a single node :)
delete node /book/author[last()]

(: Delete multiple nodes :)
delete nodes //comment

(: Delete by condition :)
delete nodes //product[discontinued = true()]

(: Delete attributes :)
delete node /book/@draft
```

## Error Codes

- `XUTY0007` — Target is not a sequence of nodes

## See Also

- [insert](insert.md)
- [replace](replace.md)
- [pending-update-list](pending-update-list.md)

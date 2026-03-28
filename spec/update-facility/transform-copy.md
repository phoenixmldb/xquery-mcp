---
name: transform-copy
category: update
since: "3.0"
spec_url: https://www.w3.org/TR/xquery-update-30/#id-transform
---

# Transform (Copy-Modify) Expression

Creates a modified copy of a node without affecting the original, using updating expressions on the copy.

## Syntax

```
TransformExpr ::= "copy" "$" VarName ":=" ExprSingle
                  ("," "$" VarName ":=" ExprSingle)*
                  "modify" ExprSingle
                  "return" ExprSingle
```

## Semantics

1. **Copy**: Each variable is bound to a deep copy of its source expression result.
2. **Modify**: The modify expression is evaluated as an updating expression, generating a pending update list that applies to the copied nodes.
3. **Return**: The pending updates are applied to the copies, and the return expression is evaluated with the modified copies.

The original nodes are never modified. This allows updating expressions in non-updating contexts.

## Examples

```xquery
(: Simple copy-modify :)
copy $b := /book
modify replace value of node $b/title with "New Title"
return $b

(: Multiple copies :)
copy $doc := /document,
     $template := /template
modify (
  replace value of node $doc/date with string(current-date()),
  delete node $template/placeholder
)
return <result>{$doc}{$template}</result>

(: Transform within a FLWOR :)
for $book in //book
return
  copy $modified := $book
  modify (
    replace value of node $modified/price
    with $modified/price * 1.1
  )
  return $modified
```

## Error Codes

- `XUDY0014` — Modify clause targets a node that is not a copy
- `XUTY0013` — Copy source is not a single node

## See Also

- [replace](replace.md)
- [pending-update-list](pending-update-list.md)

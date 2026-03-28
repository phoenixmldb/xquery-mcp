---
name: pending-update-list
category: update
since: "3.0"
spec_url: https://www.w3.org/TR/xquery-update-30/#id-pul
---

# Pending Update List (PUL)

The pending update list is the mechanism by which update operations are collected and then applied atomically.

## Concept

XQuery Update Facility uses a deferred update model:

1. **Collection phase** — Update expressions (`insert`, `delete`, `replace`, `rename`) do not modify the data model immediately. Instead, they produce update primitives that are collected in a pending update list (PUL).

2. **Application phase** — After the entire query has been evaluated, the PUL is applied atomically. Either all updates succeed, or none are applied.

## Semantics

### Update Primitives

| Primitive | Description |
|-----------|-------------|
| `insertBefore` | Insert nodes before a target |
| `insertAfter` | Insert nodes after a target |
| `insertInto` | Insert nodes as children of a target |
| `insertIntoAsFirst` | Insert as first children |
| `insertIntoAsLast` | Insert as last children |
| `insertAttributes` | Insert attributes into an element |
| `delete` | Remove a node |
| `replaceNode` | Replace a node |
| `replaceValue` | Replace a node's value |
| `rename` | Rename a node |

### Conflict Resolution

Conflicts in the PUL are resolved as follows:
- Multiple updates to the same node are allowed as long as they are compatible.
- Conflicting updates (e.g., two `replaceNode` on the same target) raise `XUDY0016`.
- The order of non-conflicting updates is implementation-defined.

### Atomicity

The PUL is applied as an atomic operation. If any update fails, the entire PUL is rolled back.

## Examples

```xquery
(: Multiple updates produce a single PUL :)
insert node <new/> into /root,
delete node /root/old,
replace value of node /root/title with "Updated"
(: All three updates are collected and applied together :)
```

## Error Codes

- `XUDY0016` — Conflicting updates in the PUL (e.g., two replacements of the same node)
- `XUDY0017` — Conflicting rename operations
- `XUDY0021` — Resulting document violates type constraints
- `XUDY0024` — Update applied to a node not in any known document

## See Also

- [insert](insert.md)
- [delete](delete.md)
- [replace](replace.md)
- [transform-copy](transform-copy.md)

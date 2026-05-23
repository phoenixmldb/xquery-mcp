---
name: update-facility
category: example
since: "1.0"
spec_url: https://www.w3.org/TR/xquery-update-10/
---

# XQuery Update Facility — insert, replace, delete, rename

## Delete nodes

```xquery
xquery version "1.0";
(: XQuery Update expressions cannot be mixed with regular return values :)

delete nodes doc('catalog.xml')//item[@discontinued = 'true']
```

## Insert nodes

```xquery
xquery version "1.0";

(: Insert a new element as the last child of /catalog :)
insert node
  <item id="99" name="New Product" price="19.99"/>
as last into doc('catalog.xml')/catalog
```

## Replace a node value

```xquery
xquery version "1.0";

replace value of node
  doc('catalog.xml')//item[@id = '42']/@price
with '24.99'
```

## Replace an entire node

```xquery
xquery version "1.0";

replace node
  doc('catalog.xml')//item[@id = '42']
with
  <item id="42" name="Updated Product" price="24.99" in-stock="true"/>
```

## Rename a node

```xquery
xquery version "1.0";

rename node doc('catalog.xml')//product
as 'item'
```

## Multiple updates in a transform expression (XQuery 3.0+)

```xquery
xquery version "3.1";

(: copy-modify-return: non-destructive update — leaves original unchanged :)
copy $cat := doc('catalog.xml')
modify (
  delete nodes $cat//item[@discontinued = 'true'],
  for $item in $cat//item
  where xs:decimal($item/@price) lt 5.00
  return replace value of node $item/@price with '5.00'
)
return $cat
```

## What it does

The XQuery Update Facility adds five updating expressions:

- `insert node X (as first/last into | before | after) target` — inserts one
  or more nodes into the target or adjacent to it.
- `delete nodes expr` — removes all nodes in the sequence from their parent.
- `replace node target with expr` — replaces a node with a sequence.
- `replace value of node target with expr` — replaces only the string value
  (for elements, replaces text content; for attributes, replaces the value).
- `rename node target as name` — changes the element or attribute name.

The **transform expression** (`copy … modify … return`) applies updates to
a deep copy of a node, leaving the original in the database unchanged.
This is the safe, functional alternative to in-place updates.

## Common pitfalls

- Update expressions cannot be mixed with regular XQuery expressions in the
  same query body unless you use the transform (`copy/modify/return`) form.
- `delete nodes` silently does nothing if the expression is empty — no error.
- `replace value of node` on an element discards all child nodes, not just text.
- The order in which updates are applied within a `modify` block follows
  the XQuery Update semantics of a **pending update list** — all deletes
  happen before inserts conceptually. Do not rely on left-to-right ordering.

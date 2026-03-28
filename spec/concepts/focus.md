---
name: focus
category: concept
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#dt-focus
---

# Focus

The focus consists of three components of the dynamic context that describe the "current position" during expression evaluation: the context item, context position, and context size.

## Components

| Component | Accessor | Description |
|-----------|----------|-------------|
| **Context item** | `.` | The item currently being processed |
| **Context position** | `fn:position()` | The 1-based position of the context item in the sequence |
| **Context size** | `fn:last()` | The total number of items in the sequence being processed |

## Semantics

The focus is established by various expressions:

- **Path expressions** — Each step sets the focus to each node selected by the previous step.
- **Predicates** — The focus is set to each item being filtered.
- **`fn:for-each`** — Sets the context item to each item in the input sequence.
- **Initial context** — May be set by the calling application (e.g., a context document).

When no focus has been established, expressions that reference `.`, `fn:position()`, or `fn:last()` raise `XPDY0002`.

The focus is **not** set by:
- `for` clauses in FLWOR expressions (use the bound variable instead)
- `let` clauses
- Function calls (unless the function internally establishes a focus)

## Examples

```xquery
(: Focus set by path expression :)
//book/title          (: . is each book element, then each title element :)

(: Focus set by predicate :)
(1 to 10)[. mod 2 = 0]   (: . is each integer in turn :)

(: Focus with position and size :)
//item[position() = last()]  (: last item :)

(: No focus — error :)
declare function local:get-name() as xs:string {
  ./name    (: XPDY0002 if no focus established :)
};
```

## Error Codes

- `XPDY0002` — Context item, position, or size is absent

## See Also

- [dynamic-context](dynamic-context.md)
- [predicates](../expressions/predicates.md)
- [path](../expressions/path.md)

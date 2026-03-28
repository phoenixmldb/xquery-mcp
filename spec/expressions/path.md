---
name: path
category: expression
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-path-expressions
---

# Path Expressions

Path expressions navigate XML trees using a sequence of steps separated by `/` or `//`. They are inherited from XPath and form the core of XML querying.

## Syntax

```
PathExpr ::= ("/" RelativePathExpr?)
           | ("//" RelativePathExpr)
           | RelativePathExpr
RelativePathExpr ::= StepExpr (("/" | "//") StepExpr)*
StepExpr ::= PostfixExpr | AxisStep
AxisStep ::= (ReverseStep | ForwardStep) PredicateList
ForwardStep ::= (ForwardAxis NodeTest) | AbbrevForwardStep
ReverseStep ::= (ReverseAxis NodeTest) | AbbrevReverseStep
```

### Forward Axes

| Axis | Description |
|------|-------------|
| `child::` | Children of the context node (default) |
| `descendant::` | All descendants |
| `attribute::` | Attributes (abbreviated as `@`) |
| `self::` | The context node itself |
| `descendant-or-self::` | Context node and all descendants |
| `following-sibling::` | All following siblings |
| `following::` | All following nodes in document order |
| `namespace::` | Namespace nodes (deprecated) |

### Reverse Axes

| Axis | Description |
|------|-------------|
| `parent::` | Parent node (abbreviated as `..`) |
| `ancestor::` | All ancestors |
| `preceding-sibling::` | All preceding siblings |
| `preceding::` | All preceding nodes in document order |
| `ancestor-or-self::` | Context node and all ancestors |

## Semantics

A path expression starting with `/` uses the root of the tree containing the context node. `//` is shorthand for `/descendant-or-self::node()/`.

Each step is evaluated with respect to each node selected by the preceding step. The results are merged in document order with duplicates eliminated.

## Examples

```xquery
(: Absolute path :)
/bookstore/book/title

(: Relative path :)
book/author/name

(: Descendant axis shorthand :)
//title

(: Attribute access :)
//book/@isbn

(: Parent axis :)
//title/..

(: Multiple steps with axes :)
//chapter/following-sibling::chapter[1]/title

(: Self axis :)
./name

(: Ancestor axis :)
//price/ancestor::book/title

(: Combining paths :)
//book[author = "Doe"]/title | //article[author = "Doe"]/title
```

## Error Codes

- `XPTY0019` — The result of the last step in a path expression contains both nodes and non-nodes (prior to 3.1)
- `XPTY0020` — The context item in an axis step is not a node
- `XPDY0002` — The context item is absent when evaluating an axis step

## See Also

- [predicates](predicates.md)
- [focus](../concepts/focus.md)

---
name: window-clause
category: example
since: "3.0"
spec_url: https://www.w3.org/TR/xquery-31/#id-window-clause
---

# Tumbling vs sliding windows in XQuery

## Tumbling window — non-overlapping fixed-size batches

```xquery
xquery version "3.1";

(: Split a flat sequence of log entries into 10-item batches :)
let $entries := doc('log.xml')//entry
for tumbling window $w in $entries
    start $s when true()
    end   $e next $n when $e/@seq mod 10 = 0
return
  <batch first="{$w[1]/@seq}" last="{$w[last()]/@seq}"
         count="{count($w)}"/>
```

## Tumbling window — group by sentinel value

```xquery
xquery version "3.1";

(: New window starts at each "section" element :)
let $nodes := doc('report.xml')/report/*
for tumbling window $w in $nodes
    start $s when $s/self::section
return
  <section title="{$w[1]/@title}" items="{count($w) - 1}"/>
```

## Sliding window — rolling 3-item average

```xquery
xquery version "3.1";

let $prices := (10, 12, 11, 15, 14, 13, 16)
for sliding window $w in $prices
    start at $s when true()
    only end at $e when $e - $s eq 2   (: window size = 3 :)
return
  <window start="{$s}" avg="{avg($w)}"/>
```

## What it does

The window clause applies to a sequence and partitions it into windows —
subsequences defined by start and end conditions.

**Tumbling windows** are non-overlapping: the sequence is divided into
consecutive, non-overlapping windows. When a window ends, the next one
starts immediately after.

**Sliding windows** are overlapping: a new window starts at every position
that satisfies the start condition, and each window slides forward
independently. The `only` keyword restricts the result to windows that
actually reach their end condition (incomplete trailing windows are excluded).

Within the conditions, positional variables (`at $s`, `at $e`) give the
1-based position in the input sequence; `next $n` / `previous $p` provide
lookahead/lookbehind.

## Common pitfalls

- `$w` inside the `return` clause is a **sequence** of items, not a single
  item — use `$w[1]`, `count($w)`, etc.
- Without `only end`, a sliding window that never reaches its end condition
  still contributes an incomplete window to the output.
- Tumbling window `end` conditions fire on the **current** item; the next
  item becomes position 1 of the following window.
- Mixing `start at $s` positional variables with `start $s when condition`
  item variables in the same condition is valid and often useful.

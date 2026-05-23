---
name: array-flatten
category: example
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-array
---

# Arrays, array:flatten, and array:for-each

## Array literals and basic access

```xquery
xquery version "3.1";

(: Square-bracket syntax creates an array :)
let $rgb := [255, 128, 0]
return (
  $rgb(1),            (: 1-based index — returns 255 :)
  $rgb?2,             (: shorthand lookup — returns 128 :)
  array:size($rgb),   (: 3 :)
  $rgb?*              (: all members as a sequence: 255 128 0 :)
)
```

## array:flatten — deep flattening of nested arrays

```xquery
xquery version "3.1";
import module namespace array = "http://www.w3.org/2005/xpath-functions/array";

let $nested := [[1, 2], [3, [4, 5]], 6]
return array:flatten($nested)
(: Returns the sequence: 1 2 3 4 5 6 :)
```

## array:for-each — transform each member

```xquery
xquery version "3.1";
import module namespace array = "http://www.w3.org/2005/xpath-functions/array";

let $prices := [10.0, 25.5, 7.99]
let $with-tax := array:for-each($prices, function($p) { $p * 1.20 })
return $with-tax?*    (: 12.0 30.6 9.588 :)
```

## Building an array from a sequence

```xquery
xquery version "3.1";
import module namespace array = "http://www.w3.org/2005/xpath-functions/array";

(: array:join wraps each item in its own 1-member array and joins :)
let $items := ("a", "b", "c")
let $arr := array{ $items }           (: [a, b, c] :)
let $arr2 := array:join(
  for $i in $items return [$i, upper-case($i)]
)                                     (: [a, A, b, B, c, C] :)
return ($arr, $arr2)
```

## What it does

XQuery 3.1 arrays are ordered sequences of members where each member can
be **any XDM value**, including a sequence. This is the key difference from
regular XQuery sequences: `(1, 2, 3)` has three atomic items, but `[1, 2, 3]`
is one array containing three members.

`array:flatten` recursively unnests arrays: any member that is itself an
array is replaced by its members, to any depth.

`array:for-each` applies a function to each member and returns a new array
of the results. The result preserves array size.

`$arr?*` is the "unboxing" operator — it converts all members to a flat
sequence.

## Common pitfalls

- `$arr(0)` is an error — XQuery arrays are **1-based**, not 0-based.
- `$arr?*` loses sequence boundaries between members: if a member is itself
  a sequence, `?*` flattens those too. Use `array:for-each` to preserve structure.
- Arrays are **not** XML nodes — they cannot appear in element content
  directly. Serialize them first (`serialize()` or `string-join($arr?*, ',')`).
- `array:flatten` flattens nested **arrays**, not sequences. If you want to
  collapse nested sequences, use `fn:subsequence` or iteration instead.

---
name: map-constructor
category: example
since: "3.1"
spec_url: https://www.w3.org/TR/xquery-31/#id-map-constructors
---

# Map literal, map:merge, and map:put

## Map literal constructor

```xquery
xquery version "3.1";

(: Construct a map literal with mixed-type values :)
let $person := map{
  "name"  : "Alice",
  "age"   : 30,
  "roles" : ("admin", "editor"),
  "active": true()
}
return (
  $person("name"),          (: lookup by key :)
  $person?age,              (: shorthand ?key lookup :)
  count($person?roles)      (: multi-value entry :)
)
```

## map:merge — combining maps

```xquery
xquery version "3.1";
import module namespace map = "http://www.w3.org/2005/xpath-functions/map";

let $defaults := map{ "timeout": 30, "retries": 3, "verbose": false() }
let $overrides := map{ "timeout": 60, "verbose": true() }

(: Later maps win for duplicate keys :)
let $config := map:merge(($defaults, $overrides),
                         map{ "duplicates": "use-last" })
return $config
```

## map:put — functional update (returns new map)

```xquery
xquery version "3.1";
import module namespace map = "http://www.w3.org/2005/xpath-functions/map";

let $m := map{ "a": 1, "b": 2 }
let $m2 := map:put($m, "c", 3)      (: adds entry; $m is unchanged :)
let $m3 := map:put($m2, "a", 99)    (: replaces entry :)
return map:keys($m3)                 (: ("a", "b", "c") in some order :)
```

## Iterating map entries

```xquery
xquery version "3.1";
import module namespace map = "http://www.w3.org/2005/xpath-functions/map";

let $scores := map{ "Alice": 95, "Bob": 87, "Carol": 91 }
for $name in map:keys($scores)
order by $scores($name) descending
return
  <entry name="{$name}" score="{$scores($name)}"/>
```

## What it does

XQuery 3.1 maps are immutable key-value stores where any atomic value
(string, integer, QName, etc.) can serve as a key. The `map{}` literal
syntax creates a map inline. Lookup is `$m(key)` or the shorthand `$m?key`.

`map:merge` combines a sequence of maps into one. The `"duplicates"` option
controls conflict resolution: `"use-last"` (later wins), `"use-first"`,
`"combine"` (makes a sequence of all values), or `"reject"` (error on dups).

`map:put` returns a **new** map with one entry added or replaced — maps are
immutable in XQuery.

## Common pitfalls

- Maps are **unordered** — `map:keys()` returns keys in implementation-defined
  order. Always `order by` when sequence matters.
- `$m("missing-key")` returns an empty sequence `()`, not an error. Use
  `map:contains($m, "key")` to test presence.
- `map:merge` without the options map uses `"duplicates": "use-first"` by
  default in XQuery 3.1.
- The `?` lookup operator is right-associative and can chain:
  `$m?person?name` is `$m("person")("name")`.

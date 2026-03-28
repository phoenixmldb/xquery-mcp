---
name: fn-format-number
category: function
since: "3.1"
spec_url: https://www.w3.org/TR/xpath-functions-31/#func-format-number
---

# fn:format-number

Formats a number as a string using a picture string, following the rules of the Java `DecimalFormat` class.

## Signature

`fn:format-number($value as xs:numeric?, $picture as xs:string) as xs:string`
`fn:format-number($value as xs:numeric?, $picture as xs:string, $decimal-format-name as xs:string) as xs:string`

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `$value` | `xs:numeric?` | The number to format |
| `$picture` | `xs:string` | Format picture string |
| `$decimal-format-name` | `xs:string` | Name of a declared decimal format |

## Semantics

The picture string contains sub-pictures for positive and negative numbers separated by `;`. Special characters in the picture string:

| Character | Meaning |
|-----------|---------|
| `0` | Mandatory digit |
| `#` | Optional digit |
| `.` | Decimal separator |
| `,` | Grouping separator |
| `%` | Multiply by 100, show as percentage |
| `‰` | Multiply by 1000, show as per-mille |
| `;` | Separator between positive and negative sub-pictures |

## Examples

```xquery
fn:format-number(1234.5, "#,##0.00")
(: Result: "1,234.50" :)

fn:format-number(0.75, "##0%")
(: Result: "75%" :)

fn:format-number(1234.5, "0000.000")
(: Result: "1234.500" :)

fn:format-number(-42, "#,##0;(#,##0)")
(: Result: "(42)" :)

fn:format-number(1234567, "#,###")
(: Result: "1,234,567" :)
```

## Error Codes

- `FODF1280` — Invalid decimal format name
- `FODF1310` — Invalid picture string

## See Also

- [fn-round](fn-round.md)
- [fn-number](fn-number.md)

using System.ComponentModel;
using ModelContextProtocol.Server;

namespace XQueryMcpServer;

[McpServerPromptType]
public static class PromptDefinitions
{
    [McpServerPrompt(Name = "xquery-write-flwor"), Description(
        "Walk through writing an XQuery 3.1 FLWOR expression with verification at each step.")]
    public static string WriteFLWOR(
        [Description("Shape of the input XML or JSON, e.g. '<orders><order id=...>...</order></orders>'")]
        string inputShape,
        [Description("Goal of the query, e.g. 'return orders over $100 grouped by customer'")]
        string goal) =>
        $$"""
        Help me write an XQuery 3.1 FLWOR expression.

        Input shape:
        {{inputShape}}

        Goal: {{goal}}

        Constraints:
        - Use let/for/where/order by/return clauses as appropriate
        - After writing each clause, call xquery_validate to verify syntax
        - Prefer group by over nested FLWOR for aggregation
        - Use maps and arrays for JSON output when appropriate
        - Use xquery_find_examples for any function or syntax you're unsure about
        - End with xquery_evaluate using a small synthetic input to verify the output

        Start by sketching the for/let binding and return clause skeleton.
        """;

    [McpServerPrompt(Name = "xquery-migrate-1-to-3"), Description(
        "Migrate an XQuery 1.0 query to XQuery 3.1, adopting higher-order functions, maps/arrays, and string templates where useful.")]
    public static string MigrateXQuery1to3(
        [Description("The XQuery 1.0 query to migrate")] string query) =>
        $$"""
        I want to migrate this XQuery 1.0 query to XQuery 3.1. Suggest concrete improvements where 3.1 idioms simplify the code:

        ```xquery
        {{query}}
        ```

        For each suggested change:
        - Cite the XQuery 3.1 feature it uses (higher-order functions, maps, arrays, string templates, group by, try/catch, etc.)
        - Show the before/after
        - Verify the change with xquery_validate

        Be conservative — only suggest changes that clearly improve readability or expressiveness. Don't churn for the sake of using new features.

        Use xquery_compare_versions to confirm any feature is in 3.1 (not a later draft).
        """;

    [McpServerPrompt(Name = "xquery-debug-query"), Description(
        "Debug an XQuery expression that's producing wrong results or erroring.")]
    public static string DebugQuery(
        [Description("The XQuery expression to debug")] string query,
        [Description("The source XML or JSON input")] string inputXml,
        [Description("The error code or wrong-result description")] string problem) =>
        $$"""
        Help me debug this XQuery expression.

        Query:
        ```xquery
        {{query}}
        ```

        Input:
        ```xml
        {{inputXml}}
        ```

        Problem: {{problem}}

        Steps:
        1. Run xquery_evaluate to capture the actual error or result
        2. If there's an error code, run xquery_explain_error with it
        3. If the result is wrong, isolate the failing clause and evaluate sub-expressions individually
        4. Propose a fix and verify with xquery_evaluate using a minimal example

        Don't guess — verify each hypothesis with a tool call.
        """;

    [McpServerPrompt(Name = "xquery-write-test"), Description(
        "Author an xquery_evaluate invocation given a query and an expected output description.")]
    public static string WriteTest(
        [Description("The XQuery expression to test")] string query,
        [Description("Description of the expected behavior")] string expectedBehavior) =>
        $$"""
        Author a test for this XQuery expression:

        ```xquery
        {{query}}
        ```

        Expected behavior: {{expectedBehavior}}

        Generate:
        1. A minimal source XML or JSON that exercises the behavior
        2. The exact expected result (canonical form, no whitespace assumptions)
        3. An xquery_evaluate call ready to run to verify the result

        After the test passes, suggest 2 edge cases worth additional tests.
        """;
}

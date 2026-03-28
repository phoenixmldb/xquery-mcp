using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using ModelContextProtocol.Server;

namespace XQueryMcpServer;

[McpServerToolType]
public static class UtilityTools
{
    [McpServerTool(Name = "xml_validate_schema"), Description(
        "Validate XML against an XSD schema. Returns validation errors or confirms validity.")]
    public static string ValidateSchema(
        [Description("XML document to validate")] string xml,
        [Description("XSD schema to validate against")] string xsd)
    {
        try
        {
            var schemas = new XmlSchemaSet();
            using var schemaReader = new StringReader(xsd);
            schemas.Add(XmlSchema.Read(schemaReader, null)!);
            schemas.Compile();

            var errors = new List<string>();
            var settings = new XmlReaderSettings
            {
                Schemas = schemas,
                ValidationType = ValidationType.Schema,
            };
            settings.ValidationEventHandler += (_, e) =>
            {
                var prefix = e.Severity == XmlSeverityType.Error ? "Error" : "Warning";
                errors.Add($"  {prefix} (line {e.Exception.LineNumber}, col {e.Exception.LinePosition}): {e.Message}");
            };

            using var xmlReader = XmlReader.Create(new StringReader(xml), settings);
            while (xmlReader.Read()) { }

            if (errors.Count == 0)
                return "Valid: XML conforms to the schema.";

            var sb = new StringBuilder();
            sb.AppendLine($"Found {errors.Count} validation issue(s):");
            foreach (var error in errors)
                sb.AppendLine(error);
            return sb.ToString();
        }
        catch (XmlSchemaException ex)
        {
            return $"Schema error: {ex.Message}";
        }
        catch (XmlException ex)
        {
            return $"XML parsing error at line {ex.LineNumber}, col {ex.LinePosition}: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Validation error: {ex.Message}";
        }
    }

    [McpServerTool(Name = "xml_format"), Description(
        "Pretty-print XML with indentation.")]
    public static string FormatXml(
        [Description("XML to format")] string xml)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true,
                NewLineOnAttributes = false,
            };

            using var writer = XmlWriter.Create(sb, settings);
            doc.WriteTo(writer);
            writer.Flush();

            return sb.ToString();
        }
        catch (XmlException ex)
        {
            return $"XML parsing error at line {ex.LineNumber}, col {ex.LinePosition}: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Format error: {ex.Message}";
        }
    }
}

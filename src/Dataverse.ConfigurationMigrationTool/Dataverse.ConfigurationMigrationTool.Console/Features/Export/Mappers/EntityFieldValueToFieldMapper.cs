using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk;
using System.Globalization;
using System.Web;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
public class EntityFieldValueToFieldMapper : IMapper<(FieldSchema, object), Field>
{
    public Field Map((FieldSchema, object) source)
    {
        var (fieldSchema, value) = source;
        var fieldResult = new Field
        {
            Name = fieldSchema.Name,
        };
        if (value == null)
        {
            return null;
        }
        if (value is EntityReference reference)
        {
            fieldResult.Lookupentity = reference.LogicalName;
            fieldResult.Lookupentityname = reference.Name;
            fieldResult.Value = reference.Id.ToString();
            return fieldResult;
        }
        if (value is OptionSetValue optionSetValue)
        {
            fieldResult.Value = optionSetValue.Value.ToString();
            return fieldResult;
        }
        if (value is bool booleanValue)
        {
            fieldResult.Value = booleanValue ? "True" : "False";
            return fieldResult;
        }
        if (value is DateTime dateTimeValue)
        {
            fieldResult.Value = dateTimeValue.ToString("o"); // ISO 8601 format
            return fieldResult;
        }
        if (value is Guid guidValue)
        {
            fieldResult.Value = guidValue.ToString();
            return fieldResult;
        }
        if (value is Money moneyValue)
        {
            fieldResult.Value = moneyValue.Value.ToString(CultureInfo.InvariantCulture);
            return fieldResult;
        }
        if (value is decimal decimalValue)
        {
            fieldResult.Value = decimalValue.ToString(CultureInfo.InvariantCulture);
            return fieldResult;
        }
        if (value is double doubleValue)
        {
            fieldResult.Value = doubleValue.ToString(CultureInfo.InvariantCulture);
            return fieldResult;
        }
        var str = value?.ToString() ?? string.Empty;
        fieldResult.Value = HttpUtility.HtmlEncode(str);
        return fieldResult;
    }
}

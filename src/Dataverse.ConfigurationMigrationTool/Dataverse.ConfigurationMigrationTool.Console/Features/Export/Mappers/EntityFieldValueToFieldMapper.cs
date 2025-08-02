using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Web;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
public class EntityFieldValueToFieldMapper : IMapper<(AttributeMetadata, FieldSchema, object), Field>
{
    public Field Map((AttributeMetadata, FieldSchema, object) source)
    {
        var (attributeMetadata, fieldSchema, value) = source;
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
            var moneyAttr = attributeMetadata as MoneyAttributeMetadata;
            var precision = moneyAttr.PrecisionSource ?? 2;
            fieldResult.Value = moneyValue.Value.ToString();
            return fieldResult;
        }
        if (value is Decimal decimalValue)
        {
            var decimalAttr = attributeMetadata as DecimalAttributeMetadata;
            var precision = decimalAttr.Precision ?? 2;
            fieldResult.Value = decimalValue.ToString();
            return fieldResult;
        }
        if (value is double doubleValue)
        {
            var doubleAttr = attributeMetadata as DoubleAttributeMetadata;
            var precision = doubleAttr.Precision ?? 2;
            fieldResult.Value = doubleValue.ToString();
            return fieldResult;
        }
        var str = value?.ToString() ?? string.Empty;
        fieldResult.Value = HttpUtility.HtmlEncode(str);
        return fieldResult;
    }
}

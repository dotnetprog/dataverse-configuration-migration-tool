using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Mappers;

public class FieldSchemaToAttributeTypeMapper : IMapper<FieldSchema, AttributeTypeCode?>
{
    public AttributeTypeCode? Map(FieldSchema source) => source.Type switch
    {
        "string" => AttributeTypeCode.String,
        "guid" => AttributeTypeCode.Uniqueidentifier,
        "entityreference" => source.LookupType == "account|contact" ? AttributeTypeCode.Customer : AttributeTypeCode.Lookup,
        "owner" => AttributeTypeCode.Owner,
        "state" => AttributeTypeCode.State,
        "status" => AttributeTypeCode.Status,
        "decimal" => AttributeTypeCode.Decimal,
        "optionsetvalue" => AttributeTypeCode.Picklist,
        "number" => AttributeTypeCode.Integer,
        "bigint" => AttributeTypeCode.BigInt,
        "float" => AttributeTypeCode.Double,
        "bool" => AttributeTypeCode.Boolean,
        "datetime" => AttributeTypeCode.DateTime,
        "money" => AttributeTypeCode.Money,
        _ => null
    };

}

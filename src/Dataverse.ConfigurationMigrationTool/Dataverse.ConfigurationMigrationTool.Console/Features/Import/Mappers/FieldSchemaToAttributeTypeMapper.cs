using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Mappers
{
    public class FieldSchemaToAttributeTypeMapper : IMapper<FieldSchema, AttributeTypeCode?>
    {
        public AttributeTypeCode? Map(FieldSchema source)
        {
            switch (source.Type)
            {
                case "string":
                    return AttributeTypeCode.String;
                case "guid":
                    return AttributeTypeCode.Uniqueidentifier;
                case "entityreference":
                    if (source.LookupType == "account|contact")
                    {
                        return AttributeTypeCode.Customer;
                    }
                    return AttributeTypeCode.Lookup;
                case "owner":
                    return AttributeTypeCode.Owner;
                case "state":
                    return AttributeTypeCode.State;
                case "status":
                    return AttributeTypeCode.Status;
                case "decimal":
                    return AttributeTypeCode.Decimal;
                case "optionsetvalue":
                    return AttributeTypeCode.Picklist;
                case "number":
                    return AttributeTypeCode.Integer;
                case "bigint":
                    return AttributeTypeCode.BigInt;
                case "float":
                    return AttributeTypeCode.Double;
                case "bool":
                    return AttributeTypeCode.Boolean;
                case "datetime":
                    return AttributeTypeCode.DateTime;
                case "money":
                    return AttributeTypeCode.Money;
                default:
                    return null;


            }
        }
    }
}

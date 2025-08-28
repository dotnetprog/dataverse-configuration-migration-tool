using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Linq;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Mappers
{
    public class AttributeMetadataToFIeldSchemaMapper : IMapper<AttributeMetadata, FieldSchema>
    {
        public FieldSchema Map(AttributeMetadata source)
        {
            var fieldSchema = new FieldSchema()
            {
                Name = source.LogicalName,
                Displayname = source.DisplayName?.UserLocalizedLabel?.Label ?? source.DisplayName.LocalizedLabels.FirstOrDefault()?.Label,
                Customfield = source.IsCustomAttribute ?? false,
                PrimaryKey = source.IsPrimaryId ?? false,
                Type = GetFieldType(source.AttributeType),
            };
            if (source is LookupAttributeMetadata lookupAttribute)
            {
                fieldSchema.LookupType = string.Join("|", lookupAttribute.Targets);
            }
            return fieldSchema;

        }
        private static string GetFieldType(AttributeTypeCode? typecode)
        {
            if (typecode == null)
                return "Unknown";
            switch (typecode.Value)
            {
                case AttributeTypeCode.BigInt:
                    return "bigint";
                case AttributeTypeCode.Boolean:
                    return "bool";
                case AttributeTypeCode.Lookup:
                case AttributeTypeCode.Customer:
                    return "entityreference";
                case AttributeTypeCode.DateTime:
                    return "datetime";
                case AttributeTypeCode.Decimal:
                    return "decimal";
                case AttributeTypeCode.Double:
                    return "float";
                case AttributeTypeCode.Integer:
                    return "number";
                case AttributeTypeCode.Money:
                    return "money";
                case AttributeTypeCode.Picklist:
                    return "optionsetvalue";
                case AttributeTypeCode.State:
                    return "state";
                case AttributeTypeCode.Status:
                    return "status";
                case AttributeTypeCode.String:
                    return "string";
                case AttributeTypeCode.Uniqueidentifier:
                    return "guid";
                case AttributeTypeCode.Owner:
                    return "owner";
            }
            throw new NotImplementedException($"{typecode} attribute type is not implemented in {nameof(AttributeMetadataToFIeldSchemaMapper)}");
        }

    }
}

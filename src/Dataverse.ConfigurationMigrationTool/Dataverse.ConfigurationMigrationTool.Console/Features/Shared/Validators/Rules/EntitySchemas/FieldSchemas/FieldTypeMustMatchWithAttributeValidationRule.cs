using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Mappers;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.FieldSchemas;

public class FieldTypeMustMatchWithAttributeValidationRule : IFieldSchemaValidationRule
{
    private static IMapper<FieldSchema, AttributeTypeCode?> AttributeTypeMapper = new FieldSchemaToAttributeTypeMapper();
    public async Task<RuleResult> Validate(FieldSchema fieldSchema, AttributeMetadata attributeMetadata)
    {
        var schemafieldtype = AttributeTypeMapper.Map(fieldSchema);
        if (schemafieldtype == null)
        {
            return RuleResult.Failure($"Schema Field type {fieldSchema.Type} is not currently supported.");

        }
        if (schemafieldtype == AttributeTypeCode.String && attributeMetadata.AttributeType == AttributeTypeCode.Memo)
        {
            // Special case for Memo, which is a long text field in Dataverse
            return RuleResult.Success();
        }
        if (schemafieldtype.Value != attributeMetadata.AttributeType)
        {
            return RuleResult.Failure($"Attribute {fieldSchema.Name} is type of {schemafieldtype} but it's expected to be {attributeMetadata.AttributeType}");
        }
        return RuleResult.Success();
    }
}

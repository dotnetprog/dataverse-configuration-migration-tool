using Dataverse.ConfigurationMigrationTool.Console.Common;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.FieldSchemas
{
    public class LookupFieldsTargetsMustMatchValidationRule : IFieldSchemaValidationRule
    {
        public async Task<RuleResult> Validate(FieldSchema fieldSchema, AttributeMetadata attributeMetadata)
        {
            if (attributeMetadata is LookupAttributeMetadata lookupMD &&
                lookupMD.AttributeType != AttributeTypeCode.Owner &&
                !lookupMD.Targets.AreEnumerablesEqualIgnoreOrder(fieldSchema.LookupType?.Split('|') ?? Array.Empty<string>()))
            {
                return RuleResult.Failure($"LookupAttribute {fieldSchema.Name} targets {fieldSchema.LookupType} but it's expected to target: {string.Join("|", lookupMD.Targets)}");

            }
            return RuleResult.Success();
        }
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.FieldSchemas
{
    public interface IFieldSchemaValidationRule
    {
        Task<RuleResult> Validate(FieldSchema fieldSchema, AttributeMetadata attributeMetadata);
    }
}

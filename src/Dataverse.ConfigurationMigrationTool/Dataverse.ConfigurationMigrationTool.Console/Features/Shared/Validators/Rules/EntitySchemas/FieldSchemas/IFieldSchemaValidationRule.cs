using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.FieldSchemas;

public interface IFieldSchemaValidationRule
{
    Task<RuleResult> Validate(FieldSchema fieldSchema, AttributeMetadata attributeMetadata);
}

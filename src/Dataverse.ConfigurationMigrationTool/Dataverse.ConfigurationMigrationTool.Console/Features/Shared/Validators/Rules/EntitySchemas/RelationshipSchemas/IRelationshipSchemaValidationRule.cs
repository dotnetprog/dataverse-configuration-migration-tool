using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.RelationshipSchemas
{
    public interface IRelationshipSchemaValidationRule
    {
        Task<RuleResult> Validate(string SourceEntityName, RelationshipSchema relationshipSchema, ManyToManyRelationshipMetadata relationshipMetadata);
    }
}

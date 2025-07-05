using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.RelationshipSchemas
{
    public interface IRelationshipSchemaValidationRule
    {
        Task<RuleResult> Validate(string SourceEntityName, RelationshipSchema relationshipSchema, ManyToManyRelationshipMetadata relationshipMetadata);
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.RelationshipSchemas
{
    public class TargetEntityNameMustMatchValidationRule : IRelationshipSchemaValidationRule
    {
        public async Task<RuleResult> Validate(string SourceEntityName, RelationshipSchema relationshipSchema, ManyToManyRelationshipMetadata relationshipMetadata)
        {
            if (relationshipMetadata.Entity2LogicalName != relationshipSchema.M2mTargetEntity)
            {
                return RuleResult.Failure($"ManyToMany Relationship Table {relationshipSchema.Name} Targets Entity is {relationshipSchema.M2mTargetEntity} but it's expected to be {relationshipMetadata.Entity2LogicalName}");

            }
            return RuleResult.Success();
        }
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.RelationshipSchemas
{
    public class SourceEntityNameMustMatchValidationRule : IRelationshipSchemaValidationRule
    {


        public async Task<RuleResult> Validate(string SourceEntityName, RelationshipSchema relationshipSchema, ManyToManyRelationshipMetadata relationshipMetadata)
        {
            if (relationshipMetadata.Entity1LogicalName != SourceEntityName)
            {
                return RuleResult.Failure($"ManyToMany Relationship Table {relationshipSchema.Name} Source Entity is {SourceEntityName} but it's expected to be {relationshipMetadata.Entity1LogicalName}");
            }

            return RuleResult.Success();
        }
    }
}

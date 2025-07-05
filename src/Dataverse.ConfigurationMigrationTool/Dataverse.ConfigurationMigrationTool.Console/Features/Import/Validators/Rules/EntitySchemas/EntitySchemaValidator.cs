using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.FieldSchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.RelationshipSchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas
{
    public class EntitySchemaValidator : IValidator<EntitySchema>
    {
        private readonly IMetadataService _metadataService;
        private readonly IReadOnlyCollection<IFieldSchemaValidationRule> _fieldSchemaRules;
        private readonly IReadOnlyCollection<IRelationshipSchemaValidationRule> _relationshipSchemaRules;
        public EntitySchemaValidator(IMetadataService metadataService,
            IEnumerable<IFieldSchemaValidationRule> fieldSchemaRules,
            IEnumerable<IRelationshipSchemaValidationRule> relationshipSchemaRules)
        {
            _metadataService = metadataService;
            _fieldSchemaRules = fieldSchemaRules.ToArray();
            _relationshipSchemaRules = relationshipSchemaRules.ToArray();
        }

        public async Task<ValidationResult> Validate(EntitySchema value)
        {
            var failures = new List<ValidationFailure>();
            var Result = new ValidationResult
            {
                Failures = failures
            };
            var entityMd = await _metadataService.GetEntity(value.Name);
            if (entityMd == null)
            {
                failures.Add(new ValidationFailure
                {
                    Message = $"Entity {value.Name} does not exist in target environment"
                });
                return Result;
            }
            foreach (var fieldSchema in value?.Fields?.Field)
            {
                var fieldResult = await ValidateFieldSchema(fieldSchema, entityMd);
                if (!fieldResult.IsSuccess)
                {
                    failures.Add(new ValidationFailure
                    {
                        Message = fieldResult.ErrorMessage,
                        PropertyBound = $"{value.Name}.{fieldSchema.Name}"
                    });
                }
            }

            foreach (var relationshipSchema in value?.Relationships?.Relationship)
            {
                var relationshipResult = await ValidateRelationshipSchema(relationshipSchema, entityMd);
                if (!relationshipResult.IsSuccess)
                {
                    failures.Add(new ValidationFailure
                    {
                        Message = relationshipResult.ErrorMessage,
                        PropertyBound = $"{value.Name}.{relationshipSchema.Name}"
                    });
                }
            }

            return Result;
        }
        private async Task<RuleResult> ValidateFieldSchema(FieldSchema fieldSchema, EntityMetadata entityMetadata)
        {
            var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(amd => amd.LogicalName == fieldSchema.Name);
            if (attributeMetadata == null)
            {
                return RuleResult.Failure($"Attribute '{fieldSchema.Name}' for Entity {entityMetadata.LogicalName} does not have exist in target environment");
            }
            foreach (var fieldSchemaRule in _fieldSchemaRules)
            {

                var fieldValidationResult = await fieldSchemaRule.Validate(fieldSchema, attributeMetadata);
                if (!fieldValidationResult.IsSuccess)
                {
                    return fieldValidationResult;
                }
            }
            return RuleResult.Success();
        }
        private async Task<RuleResult> ValidateRelationshipSchema(RelationshipSchema relationshipSchema, EntityMetadata entityMetadata)
        {
            var relationShipMetadata = await _metadataService.GetRelationShipM2M(relationshipSchema.Name);
            if (relationShipMetadata == null)
            {
                return RuleResult.Failure($"ManyToMany Relationship Table {relationshipSchema.Name} does not exist in target environment or it's not a M2M relationship.");
            }
            foreach (var schemaRule in _relationshipSchemaRules)
            {

                var fieldValidationResult = await schemaRule.Validate(entityMetadata.LogicalName, relationshipSchema, relationShipMetadata);
                if (!fieldValidationResult.IsSuccess)
                {
                    return fieldValidationResult;
                }
            }
            return RuleResult.Success();
        }
    }
}

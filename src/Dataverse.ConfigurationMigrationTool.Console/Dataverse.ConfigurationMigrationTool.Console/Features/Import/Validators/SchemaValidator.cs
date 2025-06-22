using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Mappers;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators
{
    public class SchemaValidator : IValidator<ImportSchema>
    {
        private IMetadataService _metadataService;
        private static IMapper<FieldSchema, AttributeTypeCode?> AttributeTypeMapper = new FieldSchemaToAttributeTypeMapper();
        public SchemaValidator(IMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public async Task<ValidationResult> Validate(ImportSchema value)
        {
            var failures = new List<ValidationFailure>();
            foreach (var entitySchema in value.Entity)
            {
                var entityMetadata = await _metadataService.GetEntity(entitySchema.Name);
                if (entityMetadata == null)
                {
                    failures.Add(new ValidationFailure
                    {
                        Message = $"Entity {entitySchema.Name} does not exist in target environment"
                    });
                    continue;
                }
                foreach (var fieldSchema in entitySchema.Fields.Field)
                {
                    var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(amd => amd.LogicalName == fieldSchema.Name);
                    if (attributeMetadata == null)
                    {
                        failures.Add(new ValidationFailure
                        {
                            Message = $"Attribute '{fieldSchema.Name}' for Entity {entitySchema.Name} does not have exist in target environment"
                        });
                        continue;
                    }
                    var schemafieldtype = AttributeTypeMapper.Map(fieldSchema);
                    if (schemafieldtype == null)
                    {
                        failures.Add(new ValidationFailure
                        {
                            Message = $"Schema Field type {fieldSchema.Type} is not currently supported."
                        });
                        continue;
                    }

                    if (schemafieldtype.Value != attributeMetadata.AttributeType)
                    {
                        failures.Add(new ValidationFailure
                        {
                            Message = $"Entity {entitySchema.Name} : Attribute {fieldSchema.Name} is type of {schemafieldtype} but it's expected to be {attributeMetadata.AttributeType}"
                        });
                        continue;
                    }
                    if (attributeMetadata is LookupAttributeMetadata lookupMD && lookupMD.Targets.First().ToLower() != fieldSchema.LookupType)
                    {
                        failures.Add(new ValidationFailure
                        {
                            Message = $"Entity {entitySchema.Name} : LookupAttribute {fieldSchema.Name} targets {fieldSchema.LookupType} but it's expected to target: {string.Join("|", lookupMD.Targets)}"
                        });
                        continue;
                    }


                }

                foreach (var relationshipSchema in entitySchema.Relationships.Relationship)
                {
                    var relationShipMetadata = await _metadataService.GetRelationShipM2M(relationshipSchema.Name);
                    if (relationShipMetadata == null)
                    {
                        failures.Add(new ValidationFailure
                        {
                            Message = $"ManyToMany Relationship {relationshipSchema.Name} does not exist in target environment or it's not a M2M relationship."
                        });
                        continue;
                    }
                    if (relationShipMetadata.Entity1LogicalName != entitySchema.Name)
                    {
                        failures.Add(new ValidationFailure
                        {
                            Message = $"ManyToMany Relationship {relationshipSchema.Name} Source Entity is {entitySchema.Name} but it's expected to be {relationShipMetadata.Entity1LogicalName}"
                        });
                        continue;

                    }
                    if (relationShipMetadata.Entity2LogicalName != relationshipSchema.M2mTargetEntity)
                    {
                        failures.Add(new ValidationFailure
                        {
                            Message = $"ManyToMany Relationship {relationshipSchema.Name} Targets Entity is {relationshipSchema.M2mTargetEntity} but it's expected to be {relationShipMetadata.Entity2LogicalName}"
                        });
                        continue;

                    }
                }


            }
        }
    }
}

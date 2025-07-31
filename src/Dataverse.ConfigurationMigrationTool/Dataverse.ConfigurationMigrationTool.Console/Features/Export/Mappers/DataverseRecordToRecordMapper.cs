using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
public class DataverseRecordToRecordMapper : IMapper<(EntityMetadata, EntitySchema, Entity), Record>
{
    private readonly static IMapper<(AttributeMetadata, FieldSchema, object), Field> _fieldMapper = new EntityFieldValueToFieldMapper();
    public Record Map((EntityMetadata, EntitySchema, Entity) source)
    {
        var (entityMetadata, entitySchema, entity) = source;
        var record = new Record()
        {
            Id = entity.Id,
            Field = new List<Field>()
        };

        foreach (var fieldSchema in entitySchema.Fields.Field)
        {
            if (entity.Contains(fieldSchema.Name))
            {
                var fieldValue = entity[fieldSchema.Name];
                var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(a => a.LogicalName == fieldSchema.Name);
                var field = _fieldMapper.Map((attributeMetadata, fieldSchema, fieldValue));
                if (field == null)
                {
                    continue;
                }
                record.Field.Add(field);
            }
        }
        return record;
    }
}


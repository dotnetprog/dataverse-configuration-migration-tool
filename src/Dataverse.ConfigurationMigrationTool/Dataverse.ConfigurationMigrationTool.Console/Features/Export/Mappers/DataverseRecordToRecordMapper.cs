using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
public class DataverseRecordToRecordMapper : IMapper<(EntitySchema, Entity), Record>
{
    private readonly static IMapper<(FieldSchema, object), Field> _fieldMapper = new EntityFieldValueToFieldMapper();
    public Record Map((EntitySchema, Entity) source)
    {
        var (entitySchema, entity) = source;
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
                var field = _fieldMapper.Map((fieldSchema, fieldValue));
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


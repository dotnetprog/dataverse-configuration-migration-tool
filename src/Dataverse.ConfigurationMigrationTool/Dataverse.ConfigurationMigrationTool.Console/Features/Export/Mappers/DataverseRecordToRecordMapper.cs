using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
public class DataverseRecordToRecordMapper : IMapper<(EntitySchema, Entity), Record>
{
    private bool AllowEmptyFields { get; set; }
    public DataverseRecordToRecordMapper(bool allowEmptyFields)
    {
        AllowEmptyFields = allowEmptyFields;
    }
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

            var fieldValue = entity.GetAttributeValue<object>(fieldSchema.Name);
            if (!AllowEmptyFields && fieldValue == null)
            {
                continue;
            }
            var field = _fieldMapper.Map((fieldSchema, fieldValue));

            record.Field.Add(field);

        }
        return record;
    }
}


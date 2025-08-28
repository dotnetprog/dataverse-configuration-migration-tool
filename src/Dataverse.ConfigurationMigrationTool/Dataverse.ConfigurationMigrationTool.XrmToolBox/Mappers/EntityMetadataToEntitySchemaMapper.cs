using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk.Metadata;
using System.Linq;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Mappers
{
    public class EntityMetadataToEntitySchemaMapper : IMapper<EntityMetadata, EntitySchema>
    {
        public EntitySchema Map(EntityMetadata source)
        {
            var entitySchema = new EntitySchema()
            {
                Disableplugins = false,
                Displayname = source.DisplayName?.UserLocalizedLabel?.Label ?? source.DisplayName?.LocalizedLabels.FirstOrDefault()?.Label,
                Etc = source.ObjectTypeCode ?? -1,
                Name = source.LogicalName,
                Primaryidfield = source.PrimaryIdAttribute,
                Primarynamefield = source.PrimaryNameAttribute,
            };
            return entitySchema;
        }
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Mappers
{
    public class ManyToManyRelationshipMetadataToRelationshipSchema : IMapper<ManyToManyRelationshipMetadata, RelationshipSchema>
    {
        public RelationshipSchema Map(ManyToManyRelationshipMetadata source)
        {
            return new RelationshipSchema
            {
                Name = source.IntersectEntityName,
                ManyToMany = true,
                Isreflexive = false,
                RelatedEntityName = source.IntersectEntityName,
                M2mTargetEntity = source.Entity2LogicalName,
                M2mTargetEntityPrimaryKey = source.Entity2IntersectAttribute
            };
        }
    }

}

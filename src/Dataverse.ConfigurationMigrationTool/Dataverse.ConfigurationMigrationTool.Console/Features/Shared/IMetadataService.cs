using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared
{
    public interface IMetadataService
    {
        public Task<EntityMetadata> GetEntity(string logicalName);
        public Task<ManyToManyRelationshipMetadata> GetRelationShipM2M(string logicalName);
    }
}

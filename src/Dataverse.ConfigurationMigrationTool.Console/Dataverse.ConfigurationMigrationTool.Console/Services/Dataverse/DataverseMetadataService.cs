using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse
{
    public class DataverseMetadataService : IMetadataService
    {
        private readonly IOrganizationServiceAsync2 _organizationServiceAsync2;

        public DataverseMetadataService(IOrganizationServiceAsync2 organizationServiceAsync2)
        {
            _organizationServiceAsync2 = organizationServiceAsync2;
        }

        public async Task<EntityMetadata> GetEntity(string logicalName)
        {
            var request = new RetrieveEntityRequest
            {
                LogicalName = logicalName,
                EntityFilters = EntityFilters.All,
                RetrieveAsIfPublished = false
            };
            var response = (await _organizationServiceAsync2.ExecuteAsync(request)) as RetrieveEntityResponse;
            return response.EntityMetadata;
        }

        public async Task<ManyToManyRelationshipMetadata> GetRelationShipM2M(string logicalName)
        {
            var request = new RetrieveRelationshipRequest
            {
                Name = logicalName,
            };
            var response = (await _organizationServiceAsync2.ExecuteAsync(request)) as RetrieveRelationshipResponse;
            return (ManyToManyRelationshipMetadata)response.RelationshipMetadata;
        }
    }
}

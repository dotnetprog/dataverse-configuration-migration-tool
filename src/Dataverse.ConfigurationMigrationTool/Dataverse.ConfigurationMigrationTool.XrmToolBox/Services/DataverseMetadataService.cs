using Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain.Abstraction;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Services
{
    public class DataverseMetadataService : IMetadataService
    {
        private readonly IOrganizationService _orgService;
        private List<EntityMetadata> _entityMetadataCache = new List<EntityMetadata>();
        public DataverseMetadataService(IOrganizationService orgService)
        {
            _orgService = orgService;
        }

        public IEnumerable<EntityMetadata> GetAllEntityMetadata()
        {
            if (_entityMetadataCache.Any())
            {
                return _entityMetadataCache;
            }
            var request = new RetrieveAllEntitiesRequest
            {
                EntityFilters = EntityFilters.All,
                RetrieveAsIfPublished = true
            };
            var response = (RetrieveAllEntitiesResponse)_orgService.Execute(request);
            _entityMetadataCache = response.EntityMetadata.ToList();
            return _entityMetadataCache;
        }

        public EntityMetadata GetEntityMetadata(string entityLogicalName)
        {
            var cachedValue = _entityMetadataCache.FirstOrDefault(e => e.LogicalName == entityLogicalName);
            if (cachedValue != null)
            {
                return cachedValue;
            }
            var request = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.All,
                LogicalName = entityLogicalName,
                RetrieveAsIfPublished = true
            };
            var response = (RetrieveEntityResponse)_orgService.Execute(request);
            return response.EntityMetadata;
        }
    }
}

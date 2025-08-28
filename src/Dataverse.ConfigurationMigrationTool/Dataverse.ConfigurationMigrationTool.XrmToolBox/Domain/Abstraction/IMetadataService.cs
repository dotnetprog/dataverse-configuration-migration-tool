using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain.Abstraction
{
    public interface IMetadataService
    {
        EntityMetadata GetEntityMetadata(string entityLogicalName);

        IEnumerable<EntityMetadata> GetAllEntityMetadata();
    }
}

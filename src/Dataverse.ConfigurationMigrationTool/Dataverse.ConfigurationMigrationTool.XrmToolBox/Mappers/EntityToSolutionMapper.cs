using Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Mappers
{
    public class EntityToSolutionMapper : IMapper<Entity, Solution>
    {
        public Solution Map(Entity source)
        {
            return new Solution
            {
                Id = source.Id,
                UniqueName = source.GetAttributeValue<string>("uniquename"),
                FriendlyName = source.GetAttributeValue<string>("friendlyname"),
                Version = source.GetAttributeValue<string>("version"),
                IsManaged = source.GetAttributeValue<bool?>("ismanaged") ?? false
            };
        }
    }
}

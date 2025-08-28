using Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Mappers
{
    public class EntityToSolutionComponentMapper : IMapper<Entity, SolutionEntityComponent>
    {
        public SolutionEntityComponent Map(Entity source)
        {
            return new SolutionEntityComponent
            {
                EntityName = source.GetAttributeValue<string>("logicalname"),
                DisplayEntityName = source.GetAttributeValue<string>("originallocalizedname") ?? source.GetAttributeValue<string>("name") // Fallback to logical name if localized name is not available
            };
        }
    }

}

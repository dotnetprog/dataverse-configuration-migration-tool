using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Common
{
    public static class EntityExtensions
    {
        public static Entity GetAliasedEntity(this Entity entity, string alias)
        {
            var aliasedEntity = new Entity();
            foreach (var attribute in entity.Attributes)
            {
                if (attribute.Key.StartsWith(alias + "."))
                {
                    var attributeName = attribute.Key.Substring(alias.Length + 1);
                    aliasedEntity[attributeName] = ((AliasedValue)attribute.Value)?.Value;
                }
            }

            return aliasedEntity;
        }
    }
}

using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class EntityReferenceValueConverter : BaseValueConverter<EntityReference>
    {
        const string LookupProperty = "lookuptype";

        protected override EntityReference ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {

            var lookupType = (ExtraProperties?.ContainsKey(LookupProperty) ?? false) ? ExtraProperties[LookupProperty] : string.Empty;
            if (string.IsNullOrEmpty(lookupType)) { return null; }
            if (Guid.TryParse(value, out var id))
            {
                return new EntityReference(lookupType, id);
            }


            return null;
        }
    }
}

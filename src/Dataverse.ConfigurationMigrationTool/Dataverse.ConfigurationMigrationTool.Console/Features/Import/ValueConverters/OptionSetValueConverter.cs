using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class OptionSetValueConverter : BaseValueConverter<OptionSetValue>
    {
        protected override OptionSetValue ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {
            if (int.TryParse(value, out var optionSetValue))
            {
                return new OptionSetValue(optionSetValue);
            }
            return null;
        }
    }
}

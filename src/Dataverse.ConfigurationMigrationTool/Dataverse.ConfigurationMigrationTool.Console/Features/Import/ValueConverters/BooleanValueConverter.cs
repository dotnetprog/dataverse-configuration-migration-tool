
namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class BooleanValueConverter : BaseValueConverter<bool?>
    {
        protected override bool? ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {
            if (bool.TryParse(value, out var boolValue)) { return boolValue; }
            return null;
        }
    }
}

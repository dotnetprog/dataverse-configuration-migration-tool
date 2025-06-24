
namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class IntegerValueConverter : BaseValueConverter<int?>
    {
        protected override int? ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {
            if (int.TryParse(value, out var intValue)) { return intValue; }
            return null;
        }
    }
}

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class DoubleValueConverter : BaseValueConverter<double?>
    {
        protected override double? ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {
            if (double.TryParse(value, out var doubleValue)) { return doubleValue; }
            return null;
        }
    }
}

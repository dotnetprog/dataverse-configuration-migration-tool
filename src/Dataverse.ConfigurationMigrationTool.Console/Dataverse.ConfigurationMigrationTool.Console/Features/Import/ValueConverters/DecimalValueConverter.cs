namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class DecimalValueConverter : BaseValueConverter<decimal?>
    {
        protected override decimal? ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {
            if (decimal.TryParse(value, out decimal output))
            {
                return output;
            }
            return null;
        }
    }
}

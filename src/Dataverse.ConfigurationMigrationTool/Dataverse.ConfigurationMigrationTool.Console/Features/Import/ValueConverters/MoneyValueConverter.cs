using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class MoneyValueConverter : BaseValueConverter<Money>
    {
        protected override Money ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {
            if (decimal.TryParse(value, out decimal output))
            {
                return new Money(output);
            }
            return null;
        }
    }
}

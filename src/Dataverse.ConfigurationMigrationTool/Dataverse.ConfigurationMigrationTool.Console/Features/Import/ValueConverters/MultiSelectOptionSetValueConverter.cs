using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
public class MultiSelectOptionSetValueConverter : BaseValueConverter<OptionSetValueCollection>
{
    protected override OptionSetValueCollection ConvertValue(string value, Dictionary<string, string> ExtraProperties)
    {
        EnsureValueIsValidBeforeProcessing(value);
        var result = new OptionSetValueCollection();
        var intValues = value.Split(',').Select(v => new OptionSetValue(int.Parse(v)));
        result.AddRange(intValues);
        return result;
    }
    private static void EnsureValueIsValidBeforeProcessing(string value)
    {
        var values = value.Split(',');
        foreach (var val in values)
        {
            if (!int.TryParse(val, out _))
            {
                throw new FormatException($"Value '{val}' is not a valid integer for MultiSelectOptionSet.");
            }
        }
    }
}

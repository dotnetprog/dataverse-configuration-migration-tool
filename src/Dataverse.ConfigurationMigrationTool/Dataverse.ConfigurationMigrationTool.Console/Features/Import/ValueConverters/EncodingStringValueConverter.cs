using System.Web;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
public class EncodingStringValueConverter : BaseValueConverter<string>
{
    protected override string ConvertValue(string value, Dictionary<string, string> ExtraProperties)
    {
        return HttpUtility.HtmlDecode(value);
    }
}

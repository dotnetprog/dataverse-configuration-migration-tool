
namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class GuidValueConverter : BaseValueConverter<Guid?>
    {
        protected override Guid? ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {
            if (Guid.TryParse(value, out Guid valueGuid)) return valueGuid;
            return null;
        }
    }
}

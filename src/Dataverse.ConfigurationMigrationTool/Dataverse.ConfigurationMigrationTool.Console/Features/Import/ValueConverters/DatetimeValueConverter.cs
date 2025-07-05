namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class DatetimeValueConverter : BaseValueConverter<DateTime?>
    {
        protected override DateTime? ConvertValue(string value, Dictionary<string, string> ExtraProperties)
        {
            if (DateTime.TryParse(value, out var dateTime)) { return dateTime; }
            return null;
        }
    }
}

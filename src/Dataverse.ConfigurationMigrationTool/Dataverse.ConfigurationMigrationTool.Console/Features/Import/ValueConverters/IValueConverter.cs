namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;

public interface IValueConverter
{
    object Convert(string value, Dictionary<string, string> ExtraProperties = null);
}
public interface IMainConverter
{
    T Convert<T>(string value, Dictionary<string, string> ExtraProperties = null);
}

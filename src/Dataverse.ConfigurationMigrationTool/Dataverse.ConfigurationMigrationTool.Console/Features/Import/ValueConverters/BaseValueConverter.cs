namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public abstract class BaseValueConverter<T> : IValueConverter
    {
        public object Convert(string value, Dictionary<string, string> ExtraProperties = null)
        {
            return ConvertValue(value, ExtraProperties ?? new Dictionary<string, string>());
        }
        protected abstract T ConvertValue(string value, Dictionary<string, string> ExtraProperties);
    }
}

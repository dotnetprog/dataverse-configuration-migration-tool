using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class FakeValueConverter<T> : BaseValueConverter<T>
{
    public T Value { get; set; }
    protected override T ConvertValue(string value, Dictionary<string, string> ExtraProperties)
    {
        // This is a fake converter that simply returns the default value of T
        return Value;
    }
}


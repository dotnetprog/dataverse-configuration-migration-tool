

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
public class ReflectionMainConverter : IMainConverter
{
    private readonly IEnumerable<Type> valueConvertertypes;
    private readonly Dictionary<Type, IValueConverter> cachedConverters = new Dictionary<Type, IValueConverter>();
    public ReflectionMainConverter(IEnumerable<Type> valueConvertertypes)
    {
        this.valueConvertertypes = valueConvertertypes;
    }

    public T Convert<T>(string value, Dictionary<string, string> ExtraProperties = null)
    {
        var outputType = typeof(T);
        if (cachedConverters.ContainsKey(outputType))
        {
            return (T)cachedConverters[outputType].Convert(value, ExtraProperties);
        }
        var converterType = valueConvertertypes.Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(BaseValueConverter<T>))).FirstOrDefault();
        var converter = (IValueConverter)Activator.CreateInstance(converterType);
        cachedConverters[outputType] = converter;
        return (T)converter.Convert(value, ExtraProperties);
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public class DataverseValueConverter : IDataverseValueConverter
    {
        private readonly IEnumerable<Type> valueConvertertypes;
        private readonly Dictionary<Type, IValueConverter> cachedConverters = new Dictionary<Type, IValueConverter>();
        public DataverseValueConverter(IEnumerable<Type> valueConvertertypes)
        {
            this.valueConvertertypes = valueConvertertypes;
        }



        public object Convert(AttributeMetadata attributeMetadata, Field field)
        {
            var value = field.Value;
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            switch (attributeMetadata.AttributeType.Value)
            {
                case AttributeTypeCode.String:
                    return value;
                case AttributeTypeCode.Picklist:
                case AttributeTypeCode.State:
                case AttributeTypeCode.Status:
                    return GetConverter<OptionSetValue>(value).Convert(value);
                case AttributeTypeCode.Boolean:
                    return GetConverter<bool?>(value).Convert(value);
                case AttributeTypeCode.Money:
                    return GetConverter<Money>(value).Convert(value);
                case AttributeTypeCode.Uniqueidentifier:
                    return GetConverter<Guid?>(value).Convert(value);
                case AttributeTypeCode.Integer:
                case AttributeTypeCode.BigInt:
                    return GetConverter<int?>(value).Convert(value);
                case AttributeTypeCode.DateTime:
                    return GetConverter<DateTime?>(value).Convert(value);
                case AttributeTypeCode.Decimal:
                    return GetConverter<decimal?>(value).Convert(value);
                case AttributeTypeCode.Double:
                    return GetConverter<double?>(value).Convert(value);
                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                    var properties = new Dictionary<string, string>()
                    {
                        ["lookuptype"] = field.Lookupentity
                    };
                    return GetConverter<EntityReference?>(value).Convert(value, properties);
                default:
                    throw new NotImplementedException($"{attributeMetadata.AttributeType.Value} is not implemented.");

            }
        }

        private IValueConverter GetConverter<T>(string value)
        {
            var outputType = typeof(T);
            if (cachedConverters.ContainsKey(outputType))
            {
                return cachedConverters[outputType];
            }
            var converterType = valueConvertertypes.Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(BaseValueConverter<T>))).FirstOrDefault();
            var converter = (IValueConverter)Activator.CreateInstance(converterType);
            cachedConverters[outputType] = converter;
            return converter;
        }
    }
}

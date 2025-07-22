using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;

public class DataverseValueConverter : IDataverseValueConverter
{
    private readonly IMainConverter _mainConverter;
    public DataverseValueConverter(IMainConverter mainConverter)
    {
        this._mainConverter = mainConverter;
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
            case AttributeTypeCode.Memo:
                return _mainConverter.Convert<string>(value);
            case AttributeTypeCode.Picklist:
            case AttributeTypeCode.State:
            case AttributeTypeCode.Status:
                return _mainConverter.Convert<OptionSetValue>(value);
            case AttributeTypeCode.Boolean:
                return _mainConverter.Convert<bool?>(value);
            case AttributeTypeCode.Money:
                return _mainConverter.Convert<Money>(value);
            case AttributeTypeCode.Uniqueidentifier:
                return _mainConverter.Convert<Guid?>(value);
            case AttributeTypeCode.Integer:
            case AttributeTypeCode.BigInt:
                return _mainConverter.Convert<int?>(value);
            case AttributeTypeCode.DateTime:
                return _mainConverter.Convert<DateTime?>(value);
            case AttributeTypeCode.Decimal:
                return _mainConverter.Convert<decimal?>(value);
            case AttributeTypeCode.Double:
                return _mainConverter.Convert<double?>(value);
            case AttributeTypeCode.Customer:
            case AttributeTypeCode.Lookup:
                var properties = new Dictionary<string, string>()
                {
                    ["lookuptype"] = field.Lookupentity
                };
                return _mainConverter.Convert<EntityReference>(value, properties);
            default:
                throw new NotImplementedException($"{attributeMetadata.AttributeType.Value} is not implemented.");

        }
    }
}

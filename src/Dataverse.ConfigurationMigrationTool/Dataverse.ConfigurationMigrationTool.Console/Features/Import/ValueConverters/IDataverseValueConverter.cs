using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public interface IDataverseValueConverter
    {
        public object Convert(AttributeMetadata attributeMetadata, Field field);
    }
}

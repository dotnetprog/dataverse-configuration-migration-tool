using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters
{
    public interface IDataverseValueConverter
    {
        public object Convert(AttributeMetadata attributeMetadata, Field field);
    }
}

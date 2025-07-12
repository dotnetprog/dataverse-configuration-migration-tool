using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.FakeBuilders;
public class FakeAttributeMetadataBuilder
{
    private string LogicalName { get; set; }
    private FakeAttributeMetadataBuilder()
    {
    }
    public FakeAttributeMetadataBuilder WithLogicalName(string logicalName)
    {
        LogicalName = logicalName;
        return this;
    }
    public T Build<T>() where T : AttributeMetadata, new()
    {
        var attributeMetadata = new T
        {
            LogicalName = LogicalName
        };
        return attributeMetadata;
    }
    public static FakeAttributeMetadataBuilder New()
    {
        return new FakeAttributeMetadataBuilder();
    }
}

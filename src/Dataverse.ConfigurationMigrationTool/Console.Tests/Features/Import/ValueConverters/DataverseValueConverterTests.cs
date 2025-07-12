using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Dataverse.ConfigurationMigrationTool.Console.Tests.FakeBuilders;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using NSubstitute;
using Shouldly;
using System.Web;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class DataverseValueConverterTests
{
    private readonly IMainConverter mainConverter = Substitute.For<IMainConverter>();
    private readonly DataverseValueConverter converter;
    public static TheoryData<AttributeMetadata, Field, object> TestData => new()
    {
        {
            FakeAttributeMetadataBuilder.New()
            .WithLogicalName("firstname")
            .Build<StringAttributeMetadata>(),
            new() {Name ="firstname",Value = "Root &lt;&quot;&quot;&gt; ''é&amp;&amp;@ \\ /" },
            "Root <\"\"> ''é&&@ \\ /" },
        {
            FakeAttributeMetadataBuilder
            .New()
            .WithLogicalName("customertypecode")
            .Build<PicklistAttributeMetadata>(),
            new() { Name = "customertypecode", Value = "1" },
            new OptionSetValue(1)
        }
    };
    public DataverseValueConverterTests()
    {
        converter = new DataverseValueConverter(mainConverter);
    }

    [Fact]
    public void GivenAStringAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        // Arrange
        var attributeMD =
            FakeAttributeMetadataBuilder.New()
            .WithLogicalName("firstname")
            .Build<StringAttributeMetadata>();
        var field = new Field() { Name = attributeMD.LogicalName, Value = "Root &lt;&quot;&quot;&gt; ''é&amp;&amp;@ \\ /" };
        // Act
        var result = converter.Convert(attributeMD, field);
        // Assert
        result.ShouldBe(HttpUtility.HtmlDecode(field.Value));
    }
    [Fact]
    public void GivenAPicklistAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        // Arrange
        int expectedValue = 1;
        var attributeMD =
            FakeAttributeMetadataBuilder.New()
            .WithLogicalName("customertypecode")
            .Build<PicklistAttributeMetadata>();
        var field = new Field() { Name = attributeMD.LogicalName, Value = expectedValue.ToString() };
        mainConverter.Convert<OptionSetValue>(field.Value)
            .Returns(new OptionSetValue(expectedValue));
        // Act
        var result = converter.Convert(attributeMD, field);
        // Assert
        result.ShouldBeOfType<OptionSetValue>();
        ((OptionSetValue)result).Value.ShouldBe(expectedValue);
        mainConverter.Received(1)
            .Convert<OptionSetValue>(field.Value);
    }
    [Fact]
    public void GivenAIntegerAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        // Arrange
        int expectedValue = 1;
        var attributeMD =
            FakeAttributeMetadataBuilder.New()
            .WithLogicalName("customertypecode")
            .Build<IntegerAttributeMetadata>();
        var field = new Field() { Name = attributeMD.LogicalName, Value = expectedValue.ToString() };
        mainConverter.Convert<int?>(field.Value)
           .Returns(expectedValue);
        // Act
        var result = converter.Convert(attributeMD, field);
        // Assert
        result.ShouldBeOfType<int>();
        ((int?)result).ShouldBe(expectedValue);
        mainConverter.Received(1)
            .Convert<int?>(field.Value);
    }
}

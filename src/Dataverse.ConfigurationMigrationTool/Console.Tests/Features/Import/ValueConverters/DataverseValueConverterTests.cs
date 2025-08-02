using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Tests.FakeBuilders;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using NSubstitute;
using Shouldly;
using System.Linq.Expressions;
using System.Web;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class DataverseValueConverterTests
{
    private readonly IMainConverter mainConverter = Substitute.For<IMainConverter>();
    private readonly DataverseValueConverter converter;

    public DataverseValueConverterTests()
    {
        converter = new DataverseValueConverter(mainConverter);
    }

    [Fact]
    public void GivenAStringAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        var value = "Root \"\" ''é&&@ \\ /";
        var encodedValue = HttpUtility.HtmlEncode(value);
        RunTest<string, StringAttributeMetadata>(value, encodedValue);
    }
    [Fact]
    public void GivenAPicklistAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        int expectedValue = 1;
        RunTest<OptionSetValue, PicklistAttributeMetadata>(new OptionSetValue(expectedValue), expectedValue.ToString());
    }
    [Fact]
    public void GivenAIntegerAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        RunTest<int?, IntegerAttributeMetadata>(1);
    }
    [Fact]
    public void GivenABooleanAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        RunTest<bool?, BooleanAttributeMetadata>(true);
    }
    [Fact]
    public void GivenAMoneyAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        var moneyValue = 123.4m;
        RunTest<Money, MoneyAttributeMetadata>(new Money(moneyValue), moneyValue.ToString());
    }
    [Fact]
    public void GivenAGuidAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        var value = Guid.NewGuid();
        RunTest<Guid?, UniqueIdentifierAttributeMetadata>(value, value.ToString());
    }
    [Fact]
    public void GivenADatetimeAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        var value = DateTime.UtcNow;
        RunTest<DateTime?, DateTimeAttributeMetadata>(value, value.ToString("o"));
    }
    [Fact]
    public void GivenADecimalAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        var value = 123.4m;
        RunTest<decimal?, DecimalAttributeMetadata>(value);
    }
    [Fact]
    public void GivenADoubleAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        var value = 123.4;
        RunTest<double?, DoubleAttributeMetadata>(value);
    }
    [Fact]
    public void GivenALookupAttributeAndAField_WhenConverted_ThenItShouldConvertProperly()
    {
        var value = new EntityReference { Id = Guid.NewGuid(), LogicalName = "randomentity" };
        RunLookupTest(value);
    }
    [Fact]
    public void GivenAnEmptyValueRegardlessOfAttributeType_WhenConverted_ThenItShouldReturnNull()
    {
        //Arrange
        var field = new Field() { Name = "test", Value = string.Empty };
        //Act
        var result = converter.Convert(new AttributeMetadata(), field);
        //Assert
        result.ShouldBeNull();
    }
    [Fact]
    public void GivenAnUnsupportedAttributeMetadata_WhenConverted_ThenItShouldThrowProperException()
    {
        //Arrange
        var field = new Field() { Name = "test", Value = "125462" };
        var amd = new ImageAttributeMetadata();
        //Act
        Action act = () => converter.Convert(amd, field);
        //Assert
        act.ShouldThrow<NotImplementedException>()
            .Message.ShouldBe($"{amd.AttributeType.Value} is not implemented.");
    }
    private void RunTest<T, TMD>(T expectedValue, string fieldValue = null) where TMD : AttributeMetadata, new()
    {
        // Arrange
        var attributeMD =
            FakeAttributeMetadataBuilder.New()
            .WithLogicalName("randomfield")
            .Build<TMD>();
        var field = new Field() { Name = attributeMD.LogicalName, Value = fieldValue ?? expectedValue.ToString() };
        mainConverter.Convert<T>(field.Value)
           .Returns(expectedValue);
        // Act
        var result = converter.Convert(attributeMD, field);
        // Assert
        result.ShouldBe(expectedValue);
        mainConverter.Received(1)
            .Convert<T>(field.Value);
    }
    private void RunLookupTest(EntityReference reference)
    {
        // Arrange
        Expression<Predicate<Dictionary<string, string>>> extraPropertiesPredicate = d =>
                d.ContainsKey("lookuptype") &&
                d["lookuptype"] == reference.LogicalName;

        var attributeMD =
            FakeAttributeMetadataBuilder.New()
            .WithLogicalName("randomfield")
            .Build<LookupAttributeMetadata>();
        var field = new Field() { Name = attributeMD.LogicalName, Value = reference.Id.ToString(), Lookupentity = reference.LogicalName };
        mainConverter.Convert<EntityReference>(
            field.Value,
            Arg.Is(extraPropertiesPredicate)
           )
            .Returns(reference);
        // Act
        var result = converter.Convert(attributeMD, field);
        // Assert
        result.ShouldBe(reference);
        mainConverter.Received(1)
            .Convert<EntityReference>(field.Value, Arg.Is(extraPropertiesPredicate));
    }
}

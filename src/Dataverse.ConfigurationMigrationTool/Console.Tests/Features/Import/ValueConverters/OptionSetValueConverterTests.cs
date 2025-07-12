using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Microsoft.Xrm.Sdk;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class OptionSetValueConverterTests
{
    private readonly OptionSetValueConverter converter = new OptionSetValueConverter();
    [Fact]
    public void GivenAValidIntegerString_WhenConverted_ThenItShouldReturnOptionSetValue()
    {
        // Arrange
        var integerValue = 123;
        var input = integerValue.ToString();
        // Act
        var result = converter.Convert(input) as OptionSetValue;
        // Assert
        result.ShouldNotBeNull();
        result.Value.ShouldBe(integerValue);
    }
    [Fact]
    public void GivenANonIntegerString_WhenConverted_ThenItShouldReturnNull()
    {
        // Arrange
        var input = "not an integer";
        // Act
        var result = converter.Convert(input) as OptionSetValue;
        // Assert
        result.ShouldBeNull();
    }
}

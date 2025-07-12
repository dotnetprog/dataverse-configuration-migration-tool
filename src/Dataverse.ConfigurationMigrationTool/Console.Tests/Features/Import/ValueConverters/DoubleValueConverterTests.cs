using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class DoubleValueConverterTests
{
    private readonly DoubleValueConverter converter = new DoubleValueConverter();
    [Fact]
    public void GivenADoubleString_WhenConverted_ThenItShouldReturnDouble()
    {
        // Arrange
        var doubleValue = 123.45;
        var input = doubleValue.ToString();
        // Act
        var result = converter.Convert(input) as double?;
        // Assert
        result.ShouldNotBeNull();
        result.Value.ShouldBe(doubleValue);
    }
    [Fact]
    public void GivenANonDoubleString_WhenConverted_ThenItShouldReturnNull()
    {
        // Arrange
        var input = "not a number";
        // Act
        var result = converter.Convert(input) as double?;
        // Assert
        result.ShouldBeNull();
    }
}

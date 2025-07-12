using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class BooleanValueConverterTests
{
    private readonly BooleanValueConverter converter = new BooleanValueConverter();

    [Fact]
    public void GivenTrueString_WhenConverted_ThenShouldReturnTrue()
    {
        // Arrange
        var input = "true";
        // Act
        var result = converter.Convert(input) as bool?;
        // Assert
        result.Value.ShouldBeTrue();
    }
    [Fact]
    public void GivenFalseString_WhenConverted_ThenShouldReturnFalse()
    {
        // Arrange
        var input = "false";
        // Act
        var result = converter.Convert(input) as bool?;
        // Assert
        result.Value.ShouldBeFalse();
    }
    [Fact]
    public void GivenNonBooleanString_WhenConverted_ThenShouldReturnNull()
    {
        // Arrange
        var input = "hello";
        // Act
        var result = converter.Convert(input) as bool?;
        // Assert
        result.ShouldBeNull();
    }
}

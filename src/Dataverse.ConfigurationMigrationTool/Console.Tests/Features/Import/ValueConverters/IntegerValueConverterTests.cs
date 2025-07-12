using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class IntegerValueConverterTests
{
    private readonly IntegerValueConverter converter = new IntegerValueConverter();
    [Fact]
    public void GivenAValidIntegerString_WhenConverted_ThenItShouldReturnInteger()
    {
        // Arrange
        var integerValue = 123;
        var input = integerValue.ToString();
        // Act
        var result = converter.Convert(input) as int?;
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
        var result = converter.Convert(input) as int?;
        // Assert
        result.ShouldBeNull();
    }
}

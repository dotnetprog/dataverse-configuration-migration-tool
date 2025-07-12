using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class DecimalValueConverterTests
{
    private readonly DecimalValueConverter converter = new DecimalValueConverter();
    [Fact]
    public void GivenADecimalString_WhenConverted_ThenItShouldReturnDecimal()
    {
        // Arrange
        var decimalValue = 123.45m;
        var input = decimalValue.ToString();
        // Act
        var result = converter.Convert(input) as decimal?;
        // Assert
        result.ShouldNotBeNull();
        result.Value.ShouldBe(decimalValue);
    }
    [Fact]
    public void GivenANonDecimalString_WhenConverted_ThenItShouldReturnNull()
    {
        // Arrange
        var input = "not a decimal";
        // Act
        var result = converter.Convert(input) as decimal?;
        // Assert
        result.ShouldBeNull();
    }
}

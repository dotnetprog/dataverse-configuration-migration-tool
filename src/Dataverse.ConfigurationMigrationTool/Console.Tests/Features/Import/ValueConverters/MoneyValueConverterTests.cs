using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Microsoft.Xrm.Sdk;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class MoneyValueConverterTests
{
    private readonly MoneyValueConverter converter = new MoneyValueConverter();
    [Fact]
    public void GivenADecimalString_WhenConverted_ThenItShouldReturnMoney()
    {
        // Arrange
        var decimalValue = 123.45m;
        var input = decimalValue.ToString();
        // Act
        var result = converter.Convert(input) as Money;
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
        var result = converter.Convert(input) as Money;
        // Assert
        result.ShouldBeNull();
    }
}

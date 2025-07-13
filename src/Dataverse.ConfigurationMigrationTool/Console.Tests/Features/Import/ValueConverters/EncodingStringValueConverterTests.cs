using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Shouldly;
using System.Web;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class EncodingStringValueConverterTests
{
    private readonly EncodingStringValueConverter converter = new EncodingStringValueConverter();
    [Fact]
    public void GivenAnEncodedString_WhenItIsConverted_ThenItShouldDecodeProperly()
    {
        // Arrange
        var expectedValue = "Root \"\" ''é&&@ \\ /";
        // Act
        var result = converter.Convert(HttpUtility.HtmlEncode(expectedValue), null);
        // Assert
        result.ShouldBe(expectedValue);
    }
    [Fact]
    public void GivenADecodedString_WhenItIsConverted_ThenItShouldReturnSameString()
    {
        // Arrange
        var expectedValue = "Root \"\" ''é&&@ \\ /";
        // Act
        var result = converter.Convert(expectedValue, null);
        // Assert
        result.ShouldBe(expectedValue);
    }
}

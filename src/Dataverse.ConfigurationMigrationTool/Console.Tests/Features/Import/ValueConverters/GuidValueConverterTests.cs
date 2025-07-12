using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class GuidValueConverterTests
{
    private readonly GuidValueConverter converter = new GuidValueConverter();
    [Fact]
    public void GivenAValidGuidString_WhenConverted_ThenItShouldReturnGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var input = guid.ToString();

        // Act
        var result = converter.Convert(input) as Guid?;

        // Assert
        result.ShouldNotBeNull();
        result.Value.ShouldBe(guid);
    }
    [Fact]
    public void GivenANonValidGuidString_WhenConverted_ThenItShouldReturnNull()
    {
        // Arrange
        var input = "Not a guid";

        // Act
        var result = converter.Convert(input) as Guid?;

        // Assert
        result.ShouldBeNull();
    }
}

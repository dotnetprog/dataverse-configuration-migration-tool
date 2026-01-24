using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Microsoft.Xrm.Sdk;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class MultiSelectOptionSetValueConverterTests
{
    private readonly MultiSelectOptionSetValueConverter converter = new MultiSelectOptionSetValueConverter();
    [Fact]
    public void GivenAMultiSelectString_WhenConverted_ThenItShouldReturnOptionSetValueCollection()
    {
        // Arrange
        var input = "1,2,3,4";
        // Act
        var result = converter.Convert(input) as OptionSetValueCollection;
        // Assert
        result.ShouldNotBeNull();
        result.ToArray().ShouldBeEquivalentTo(new[]
        {
            new OptionSetValue(1),
            new OptionSetValue(2),
            new OptionSetValue(3),
            new OptionSetValue(4)
        });
    }
    [Fact]
    public void GivenAnInvalidMultiSelectString_WhenConverted_ThenItShouldThrowFormatException()
    {
        // Arrange
        var input = "not a number";
        // Act
        Action act = () => converter.Convert(input);
        // Assert
        act.ShouldThrow<FormatException>();
    }
}

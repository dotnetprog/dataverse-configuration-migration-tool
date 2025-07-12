using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class DatetimeValueConverterTests
{
    private readonly DatetimeValueConverter converter = new DatetimeValueConverter();
    [Fact]
    public void GivenADateTimeString_WhenConverted_ThenItShouldReturnDateTime()
    {
        // Arrange
        var date = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc);
        var input = date.ToString("o"); // ISO 8601 format
        // Act
        var result = converter.Convert(input) as DateTime?;
        // Assert
        result.ShouldNotBeNull();
        result.Value.ShouldBe(date);
    }
    [Fact]
    public void GivenANonDateTimeString_WhenConverted_ThenItShouldReturnNull()
    {
        // Arrange;
        var input = "not a date";
        // Act
        var result = converter.Convert(input) as DateTime?;
        // Assert
        result.ShouldBeNull();
    }
}

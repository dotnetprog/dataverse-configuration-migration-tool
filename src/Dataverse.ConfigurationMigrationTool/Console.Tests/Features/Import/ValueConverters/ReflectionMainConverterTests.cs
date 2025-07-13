using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using NSubstitute;
using Shouldly;
using System.Web;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class ReflectionMainConverterTests
{
    [Fact]
    public void GivenAValueConverter_WhenConvertIsCalled_ThenItShouldReturnConvertedValue()
    {
        // Arrange
        var valueConverterTypes = new List<Type> { typeof(EncodingStringValueConverter) };
        var types = Substitute.For<IEnumerable<Type>>();
        types.GetEnumerator().Returns(valueConverterTypes.GetEnumerator());
        var mainConverter = new ReflectionMainConverter(types);
        var expectedValue = "Root \"\" ''é&&@ \\ /";

        // Act
        var result = mainConverter.Convert<string>(HttpUtility.HtmlEncode(expectedValue));

        // Assert
        result.ShouldBe(expectedValue);
        types.Received(1).GetEnumerator();
    }
    [Fact]
    public void GivenAValueConverter_WhenConvertIsCalledTwiceForSameConversion_ThenItShouldCacheThe2ndTimeReturnConvertedValue()
    {
        // Arrange
        var valueConverterTypes = new List<Type> { typeof(EncodingStringValueConverter) };
        var types = Substitute.For<IEnumerable<Type>>();
        types.GetEnumerator().Returns(valueConverterTypes.GetEnumerator());
        var mainConverter = new ReflectionMainConverter(types);
        var expectedValue = "Root \"\" ''é&&@ \\ /";

        // Act
        var result = mainConverter.Convert<string>(HttpUtility.HtmlEncode(expectedValue));
        result = mainConverter.Convert<string>(HttpUtility.HtmlEncode(expectedValue));

        // Assert
        result.ShouldBe(expectedValue);
        types.Received(1).GetEnumerator();
    }
}

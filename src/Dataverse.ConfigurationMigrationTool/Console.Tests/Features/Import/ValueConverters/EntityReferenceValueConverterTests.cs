using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Microsoft.Xrm.Sdk;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.ValueConverters;
public class EntityReferenceValueConverterTests
{
    private readonly EntityReferenceValueConverter converter = new EntityReferenceValueConverter();
    [Fact]
    public void GivenValidEntityReferenceString_WhenConverted_ThenShouldReturnEntityReference()
    {
        // Arrange

        var entityReference = new EntityReference()
        {
            Id = Guid.NewGuid(),
            LogicalName = "account"
        };
        var extraProperties = new Dictionary<string, string>
        {
            { "lookuptype", entityReference.LogicalName }
        };
        // Act
        var result = converter.Convert(entityReference.Id.ToString(), extraProperties) as EntityReference;
        // Assert
        result.ShouldNotBeNull();
        result.LogicalName.ShouldBe("account");
        result.Id.ShouldBe(entityReference.Id);
    }
    [Fact]
    public void GivenValidEntityReferenceStringWithNoExtraProperties_WhenConverted_ThenShouldReturnNull()
    {
        // Arrange

        var entityReference = new EntityReference()
        {
            Id = Guid.NewGuid(),
            LogicalName = "account"
        };

        // Act
        var result = converter.Convert(entityReference.Id.ToString()) as EntityReference;
        // Assert
        result.ShouldBeNull();
    }
    [Fact]
    public void GivenNonValidGuidStringWithExtraProperties_WhenConverted_ThenShouldReturnNull()
    {
        // Arrange

        var entityReference = new EntityReference()
        {
            Id = Guid.NewGuid(),
            LogicalName = "account"
        };
        var extraProperties = new Dictionary<string, string>
        {
            { "lookuptype", entityReference.LogicalName }
        };
        // Act
        var result = converter.Convert("not a guid", extraProperties) as EntityReference;
        // Assert
        result.ShouldBeNull();
    }
}

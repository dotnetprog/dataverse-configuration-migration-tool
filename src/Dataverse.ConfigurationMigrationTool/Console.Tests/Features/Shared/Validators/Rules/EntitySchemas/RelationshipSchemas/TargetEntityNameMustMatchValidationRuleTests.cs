using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.RelationshipSchemas;
using Microsoft.Xrm.Sdk.Metadata;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Shared.Validators.Rules.EntitySchemas.RelationshipSchemas;
public class TargetEntityNameMustMatchValidationRuleTests
{
    private readonly TargetEntityNameMustMatchValidationRule _rule = new();

    [Fact]
    public async Task GivenAMatchingRelationShipMetadata_WhenValidated_ThenItShouldReturnSuccess()
    {
        // Arrange
        var relationshipSchema = new RelationshipSchema { Name = "account_contact", M2mTargetEntity = "contact" };
        var relationshipMetadata = new ManyToManyRelationshipMetadata
        {
            Entity2LogicalName = "contact"
        };
        // Act
        var result = await _rule.Validate(string.Empty, relationshipSchema, relationshipMetadata);
        // Assert
        result.IsSuccess.ShouldBeTrue();
    }
    [Fact]
    public async Task GivenANonMatchingRelationShipMetadata_WhenValidated_ThenItShouldReturnProperFailure()
    {
        // Arrange
        var relationshipSchema = new RelationshipSchema { Name = "account_contact", M2mTargetEntity = "contact" };
        var relationshipMetadata = new ManyToManyRelationshipMetadata
        {
            Entity2LogicalName = "contact1"
        };
        // Act
        var result = await _rule.Validate(string.Empty, relationshipSchema, relationshipMetadata);
        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldBe($"ManyToMany Relationship Table {relationshipSchema.Name} Targets Entity is {relationshipSchema.M2mTargetEntity} but it's expected to be {relationshipMetadata.Entity2LogicalName}");
    }
}

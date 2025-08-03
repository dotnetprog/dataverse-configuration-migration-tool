using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.RelationshipSchemas;
using Microsoft.Xrm.Sdk.Metadata;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Shared.Validators.Rules.EntitySchemas.RelationshipSchemas;
public class SourceEntityNameMustMatchValidationRuleTests
{
    private readonly SourceEntityNameMustMatchValidationRule _rule = new();

    [Fact]
    public async Task GivenAMatchingSourceEntityName_WhenValidatingRelationshipSchema_ThenItShouldReturnSuccess()
    {
        // Arrange
        var sourceEntityName = "account";
        var relationshipSchema = new RelationshipSchema { Name = "account_contact" };
        var relationshipMetadata = new ManyToManyRelationshipMetadata
        {
            Entity1LogicalName = "account"
        };
        // Act
        var result = await _rule.Validate(sourceEntityName, relationshipSchema, relationshipMetadata);
        // Assert
        result.IsSuccess.ShouldBeTrue();
    }
    [Fact]
    public async Task GivenANonMatchingSourceEntityName_WhenValidatingRelationshipSchema_ThenItShouldReturnProperFailure()
    {
        // Arrange
        var sourceEntityName = "account";
        var relationshipSchema = new RelationshipSchema { Name = "account_contact" };
        var relationshipMetadata = new ManyToManyRelationshipMetadata
        {
            Entity1LogicalName = "account1"
        };
        // Act
        var result = await _rule.Validate(sourceEntityName, relationshipSchema, relationshipMetadata);
        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldBe($"ManyToMany Relationship Table {relationshipSchema.Name} Source Entity is {sourceEntityName} but it's expected to be {relationshipMetadata.Entity1LogicalName}");
    }
}

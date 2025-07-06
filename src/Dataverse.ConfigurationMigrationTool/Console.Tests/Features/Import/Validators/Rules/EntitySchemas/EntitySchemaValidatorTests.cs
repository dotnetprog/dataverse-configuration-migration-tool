using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.FieldSchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.RelationshipSchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk.Metadata;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Validators.Rules.EntitySchemas;
public class EntitySchemaValidatorTests
{
    private readonly IMetadataService metadataService = Substitute.For<IMetadataService>();
    [Fact]
    public async Task GivenAValidEntitySchema_WhenItIsValidated_ThenItShouldReturnSuccess()
    {
        // Arrange
        var contactMd = FakeMetadata.Contact;
        metadataService.GetEntity("contact").Returns(contactMd);
        metadataService.GetRelationShipM2M(FakeSchemas.Contact.Relationships.Relationship.First().Name)
            .Returns(contactMd.ManyToManyRelationships.FirstOrDefault());
        var validFieldSchemaRule = Substitute.For<IFieldSchemaValidationRule>();
        validFieldSchemaRule.Validate(Arg.Any<FieldSchema>(), Arg.Any<AttributeMetadata>())
            .Returns(RuleResult.Success());

        var validRelationshipSchemaRule = Substitute.For<IRelationshipSchemaValidationRule>();
        validRelationshipSchemaRule.Validate(Arg.Any<string>(), Arg.Any<RelationshipSchema>(), Arg.Any<ManyToManyRelationshipMetadata>())
            .Returns(RuleResult.Success());
        var validator = CreateValidator([validFieldSchemaRule], [validRelationshipSchemaRule]);
        //Act
        var result = await validator.Validate(FakeSchemas.Contact);
        //Assert
        result.IsError.ShouldBeFalse();
        await validFieldSchemaRule.Received(2).Validate(Arg.Any<FieldSchema>(), Arg.Any<AttributeMetadata>());
        await validRelationshipSchemaRule.Received(1).Validate(Arg.Any<string>(), Arg.Any<RelationshipSchema>(), Arg.Any<ManyToManyRelationshipMetadata>());
    }





    private IValidator<EntitySchema> CreateValidator(
        IEnumerable<IFieldSchemaValidationRule> fieldSchemaRules,
        IEnumerable<IRelationshipSchemaValidationRule> relationshipSchemaRules)
    {
        return new EntitySchemaValidator(metadataService,
            fieldSchemaRules ?? Enumerable.Empty<IFieldSchemaValidationRule>(),
            relationshipSchemaRules ?? Enumerable.Empty<IRelationshipSchemaValidationRule>());
    }
}

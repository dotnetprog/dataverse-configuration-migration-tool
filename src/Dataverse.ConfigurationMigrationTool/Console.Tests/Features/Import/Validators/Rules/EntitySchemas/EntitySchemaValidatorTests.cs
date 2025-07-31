using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.FieldSchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.RelationshipSchemas;
using Microsoft.Xrm.Sdk.Metadata;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Validators.Rules.EntitySchemas;
public class EntitySchemaValidatorTests
{
    private readonly IMetadataService metadataService = Substitute.For<IMetadataService>();
    [Fact]
    public async Task GivenEntityDoesNotExist_WhenValidated_ThenShouldReturnError()
    {
        // Arrange
        metadataService.GetEntity("nonexistent").Returns((EntityMetadata)null);
        var validator = CreateValidator(null, null);

        var schema = new EntitySchema { Name = "nonexistent" };

        // Act
        var result = await validator.Validate(schema);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Failures.ShouldContain(f => f.Message == $"Entity {schema.Name} does not exist in target environment");
    }

    [Fact]
    public async Task GivenFieldDoesNotExist_WhenValidated_ThenShouldReturnError()
    {
        // Arrange
        var contactMd = FakeMetadata.Contact;
        metadataService.GetEntity("contact").Returns(contactMd);
        var missingField = new FieldSchema { Name = "missing_field" };
        var schema = new EntitySchema { Name = "contact", Fields = new FieldsSchema { Field = new List<FieldSchema> { missingField } } };
        var validator = CreateValidator(null, null);

        // Act
        var result = await validator.Validate(schema);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Failures.ShouldContain(f => f.Message == $"Attribute '{missingField.Name}' for Entity {contactMd.LogicalName} does not have exist in target environment");
    }

    [Fact]
    public async Task GivenFieldRuleFails_WhenValidated_ThenShouldReturnError()
    {
        // Arrange
        var contactMd = FakeMetadata.Contact;
        metadataService.GetEntity("contact").Returns(contactMd);
        var fieldSchema = new FieldSchema { Name = contactMd.Attributes.First().LogicalName };
        var schema = new EntitySchema { Name = "contact", Fields = new FieldsSchema { Field = new List<FieldSchema> { fieldSchema } } };
        var failedRule = Substitute.For<IFieldSchemaValidationRule>();
        failedRule.Validate(Arg.Any<FieldSchema>(), Arg.Any<AttributeMetadata>())
            .Returns(RuleResult.Failure("Field rule failed"));
        var validator = CreateValidator([failedRule], null);

        // Act
        var result = await validator.Validate(schema);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Failures.ShouldContain(f => f.Message == "Field rule failed");
    }

    [Fact]
    public async Task GivenRelationshipDoesNotExist_WhenValidated_ThenShouldReturnError()
    {
        // Arrange
        var contactMd = FakeMetadata.Contact;
        metadataService.GetEntity("contact").Returns(contactMd);
        metadataService.GetRelationShipM2M("missing_relationship").Returns((ManyToManyRelationshipMetadata)null);
        var relationshipSchema = new RelationshipSchema { Name = "missing_relationship" };
        var schema = new EntitySchema { Name = "contact", Relationships = new RelationshipsSchema { Relationship = new List<RelationshipSchema> { relationshipSchema } } };
        var validator = CreateValidator(null, null);

        // Act
        var result = await validator.Validate(schema);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Failures.ShouldContain(f => f.Message == $"ManyToMany Relationship Table {relationshipSchema.Name} does not exist in target environment or it's not a M2M relationship.");
    }

    [Fact]
    public async Task GivenRelationshipRuleFails_WhenValidated_ThenShouldReturnError()
    {
        // Arrange
        var contactMd = FakeMetadata.Contact;
        metadataService.GetEntity("contact").Returns(contactMd);
        var m2m = contactMd.ManyToManyRelationships.FirstOrDefault();
        metadataService.GetRelationShipM2M(m2m.SchemaName).Returns(m2m);
        var relationshipSchema = new RelationshipSchema { Name = m2m.SchemaName };
        var schema = new EntitySchema { Name = "contact", Relationships = new RelationshipsSchema { Relationship = new List<RelationshipSchema> { relationshipSchema } } };
        var failedRule = Substitute.For<IRelationshipSchemaValidationRule>();
        failedRule.Validate(Arg.Any<string>(), Arg.Any<RelationshipSchema>(), Arg.Any<ManyToManyRelationshipMetadata>())
            .Returns(RuleResult.Failure("Relationship rule failed"));
        var validator = CreateValidator(null, new List<IRelationshipSchemaValidationRule> { failedRule });

        // Act
        var result = await validator.Validate(schema);

        // Assert
        result.IsError.ShouldBeTrue();
        result.Failures.ShouldContain(f => f.Message == "Relationship rule failed");
    }

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

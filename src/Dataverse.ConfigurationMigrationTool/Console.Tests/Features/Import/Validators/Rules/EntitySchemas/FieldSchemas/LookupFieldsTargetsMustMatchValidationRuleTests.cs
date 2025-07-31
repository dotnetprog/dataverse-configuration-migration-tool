using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.FieldSchemas;
using Microsoft.Xrm.Sdk.Metadata;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Validators.Rules.EntitySchemas.FieldSchemas;
public class LookupFieldsTargetsMustMatchValidationRuleTests :
    BaseFieldSchemaValidationRuleTests<LookupFieldsTargetsMustMatchValidationRule>
{
    [Fact]
    public async Task GivenANonLookupSchema_WhenItIsValidated_ThenItShouldReturnSuccess()
    {
        // Arrange
        var fieldSchema = new FieldSchema
        {
            Name = "testField",
            Type = "string"
        };
        var attributeMetadata = CreateAttributeMetadata<StringAttributeMetadata>();

        // Act
        var result = await ExecuteRule(fieldSchema, attributeMetadata);

        // Assert
        result.IsSuccess.ShouldBe(true);
    }
    [Fact]
    public async Task GivenAlookupSchemaThatMatchesAttributeMetadataTargets_WhenItIsValidated_ThenItShouldReturnSuccess()
    {
        // Arrange
        var fieldSchema = new FieldSchema
        {
            Name = "testField",
            Type = "entityreference",
            LookupType = "account|contact"
        };
        var attributeMetadata = CreateAttributeMetadata<LookupAttributeMetadata>();
        attributeMetadata.Targets = new[] { "account", "contact" };

        // Act
        var result = await ExecuteRule(fieldSchema, attributeMetadata);

        // Assert
        result.IsSuccess.ShouldBe(true);
    }
    [Fact]
    public async Task GivenAlookupSchemaThatDoesNotMatchesAttributeMetadataTargets_WhenItIsValidated_ThenItShouldReturnProperFailure()
    {
        // Arrange
        var fieldSchema = new FieldSchema
        {
            Name = "testField",
            Type = "entityreference",
            LookupType = "contact"
        };
        var attributeMetadata = CreateAttributeMetadata<LookupAttributeMetadata>();
        attributeMetadata.Targets = new[] { "account", "contact" };

        // Act
        var result = await ExecuteRule(fieldSchema, attributeMetadata);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldBe($"LookupAttribute {fieldSchema.Name} targets contact but it's expected to target: account|contact");
    }
    [Fact]
    public async Task GivenAlookupSchemaWithAnAttributeCodeThatIsOwnerType_WhenItIsValidated_ThenItShouldReturnSuccess()
    {
        // Arrange
        var fieldSchema = new FieldSchema
        {
            Name = "testField",
            Type = "entityreference",
            LookupType = "owner"
        };
        var attributeMetadata = CreateAttributeMetadata<LookupAttributeMetadata>();
        typeof(LookupAttributeMetadata).GetProperty(nameof(LookupAttributeMetadata.AttributeType))!
            .SetValue(attributeMetadata, AttributeTypeCode.Owner);
        attributeMetadata.Targets = new[] { "systemuser", "team" };

        // Act
        var result = await ExecuteRule(fieldSchema, attributeMetadata);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }
}

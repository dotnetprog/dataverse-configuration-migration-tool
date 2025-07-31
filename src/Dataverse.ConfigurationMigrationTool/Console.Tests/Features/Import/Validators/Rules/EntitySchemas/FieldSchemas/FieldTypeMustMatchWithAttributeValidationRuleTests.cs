using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.FieldSchemas;
using Microsoft.Xrm.Sdk.Metadata;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Validators.Rules.EntitySchemas.FieldSchemas;
public class FieldTypeMustMatchWithAttributeValidationRuleTests : BaseFieldSchemaValidationRuleTests<FieldTypeMustMatchWithAttributeValidationRule>
{

    [Fact]
    public async Task GivenAFieldSchemaWithAttributeTypeCode_WhenItIsValidated_ThenItShouldReturnSuccess()
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
    public async Task GivenAFieldSchemaWithAttributeTypeCodeMemo_WhenItIsValidated_ThenItShouldReturnSuccess()
    {
        // Arrange
        var fieldSchema = new FieldSchema
        {
            Name = "testField",
            Type = "string"
        };
        var attributeMetadata = CreateAttributeMetadata<MemoAttributeMetadata>();
        // Act
        var result = await ExecuteRule(fieldSchema, attributeMetadata);
        // Assert
        result.IsSuccess.ShouldBe(true);
    }
    [Fact]
    public async Task GivenAnUnresolvedFieldSchemaWithAttributeTypeCode_WhenItIsValidated_ThenItShouldReturnTheProperFailure()
    {
        var fieldSchema = new FieldSchema
        {
            Name = "testField",
            Type = "string2"
        };
        var attributeMetadata = CreateAttributeMetadata<StringAttributeMetadata>();
        // Act
        var result = await ExecuteRule(fieldSchema, attributeMetadata);
        // Assert
        result.IsSuccess.ShouldBe(false);
        result.ErrorMessage.ShouldBe($"Schema Field type {fieldSchema.Type} is not currently supported.");
    }
    [Fact]
    public async Task GivenAFieldSchemaWithWrongAttributeTypeCode_WhenItIsValidated_ThenItShouldReturnTheProperFailure()
    {
        var fieldSchema = new FieldSchema
        {
            Name = "testField",
            Type = "string"
        };
        var attributeMetadata = CreateAttributeMetadata<MoneyAttributeMetadata>();
        // Act
        var result = await ExecuteRule(fieldSchema, attributeMetadata);
        // Assert
        result.IsSuccess.ShouldBe(false);
        result.ErrorMessage.ShouldBe($"Attribute {fieldSchema.Name} is type of String but it's expected to be {attributeMetadata.AttributeType}");
    }
}



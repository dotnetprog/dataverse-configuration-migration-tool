using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Mappers;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Microsoft.Xrm.Sdk.Metadata;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Mappers;
public class FieldSchemaToAttributeTypeMapperTests :
    BaseMapperTests<FieldSchemaToAttributeTypeMapper, FieldSchema, AttributeTypeCode?>
{
    public static readonly TheoryData<FieldSchema, AttributeTypeCode> TestData = new() {
        { new FieldSchema { Type = "string" }, AttributeTypeCode.String },
        { new FieldSchema { Type = "guid" }, AttributeTypeCode.Uniqueidentifier },
        { new FieldSchema { Type = "entityreference", LookupType = "account|contact" }, AttributeTypeCode.Customer },
        { new FieldSchema { Type = "entityreference", LookupType = "account" }, AttributeTypeCode.Lookup },
        { new FieldSchema { Type = "owner" }, AttributeTypeCode.Owner },
        { new FieldSchema { Type = "state" }, AttributeTypeCode.State },
        { new FieldSchema { Type = "status" }, AttributeTypeCode.Status },
        { new FieldSchema { Type = "decimal" }, AttributeTypeCode.Decimal },
        { new FieldSchema { Type = "optionsetvalue" }, AttributeTypeCode.Picklist },
        { new FieldSchema { Type = "number" }, AttributeTypeCode.Integer },
        { new FieldSchema { Type = "bigint" }, AttributeTypeCode.BigInt },
        { new FieldSchema { Type = "float" }, AttributeTypeCode.Double },
        { new FieldSchema { Type = "bool" }, AttributeTypeCode.Boolean },
        { new FieldSchema { Type = "datetime" }, AttributeTypeCode.DateTime },
        { new FieldSchema { Type = "money" }, AttributeTypeCode.Money }
    };
    [Theory]
    [MemberData(nameof(TestData))]
    public void GivenAFieldSchema_WhenItIsMapped_ThenItShouldReturnProperAttributeTypeCode(FieldSchema source, AttributeTypeCode expected)
    {
        // Act
        var result = RunTest(source);
        // Assert
        result.ShouldBe(expected);
    }
}

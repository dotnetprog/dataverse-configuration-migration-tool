using Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk;
using Shouldly;
using System.Web;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Export.Mappers;
public class EntityFieldValueToFieldMapperTests
{
    private readonly EntityFieldValueToFieldMapper _mapper = new EntityFieldValueToFieldMapper();
    private static readonly DateTime CurrentDate = DateTime.UtcNow;
    private static readonly Guid RandomGuid = Guid.NewGuid();
    public static TheoryData<FieldSchema, object, Field> TestData => new TheoryData<FieldSchema, object, Field>
    {
        {
            new FieldSchema { Name = "testField" },
            null,
            new Field { Name = "testField",IsNull=true }
        },
        {
            new FieldSchema { Name = "testLookupField" },
            new EntityReference("testEntity", RandomGuid),
            new Field { Name = "testLookupField", Lookupentity = "testEntity", Value = RandomGuid.ToString() }
        },
        {
            new FieldSchema { Name = "testOptionSetField" },
            new OptionSetValue(1),
            new Field { Name = "testOptionSetField", Value = "1" }
        },
        {
            new FieldSchema { Name = "testBooleanField" },
            true,
            new Field { Name = "testBooleanField", Value = "True" }
        },
        {
            new FieldSchema { Name = "testDateTimeField" },
            CurrentDate,
            new Field { Name = "testDateTimeField", Value = CurrentDate.ToString("o") }
        },
        {
            new FieldSchema { Name = "testGuidField" },
            RandomGuid,
            new Field { Name = "testGuidField", Value = RandomGuid.ToString() }
        },
        {
            new FieldSchema { Name = "testMoneyField" },
            new Money(100.50m),
            new Field { Name = "testMoneyField", Value = "100.50" }
        },
        {
            new FieldSchema { Name = "testDecimalField" },
            123.45m,
            new Field { Name = "testDecimalField", Value = "123.45" }
        },
        {
            new FieldSchema { Name = "testDoubleField" },
            123.456789,
            new Field { Name = "testDoubleField", Value = "123.456789" }
        },
        {
            new FieldSchema { Name = "testStringField" },
            "<>Root<>''/",
            new Field { Name = "testStringField", Value = HttpUtility.HtmlEncode("<>Root<>''/") }
        }
    };

    [Theory]
    [MemberData(nameof(TestData))]
    public void GivenAnAttributeValue_WhenItIsMappedToAField_ThenFieldShouldBeCreatedProperly(FieldSchema schema, object value, Field expected)
    {
        // Act
        var result = _mapper.Map((schema, value));
        // Assert
        if (expected == null)
        {
            result.ShouldBeNull();
        }
        else
        {
            result.ShouldBeEquivalentTo(expected);
        }
    }
}

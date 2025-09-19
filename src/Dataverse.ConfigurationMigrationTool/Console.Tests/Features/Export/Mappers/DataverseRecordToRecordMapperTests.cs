using Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
using Microsoft.Xrm.Sdk;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Export.Mappers;
public class DataverseRecordToRecordMapperTests
{
    private readonly DataverseRecordToRecordMapper _mapperWithNoEmptyFields = new DataverseRecordToRecordMapper(false);
    private readonly DataverseRecordToRecordMapper _mapperWithEmptyFields = new DataverseRecordToRecordMapper(true);
    [Fact]
    public void GivenAnEntity_WhenItIsMappedToARecordWithNoEmptyFields_ThenItTheRecordShouldBeProperplyCreated()
    {
        //Arrange
        var Schema = FakeSchemas.Account;
        var Entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["primarycontactid"] = new EntityReference("contact", Guid.NewGuid())
        };
        //Act
        var record = _mapperWithNoEmptyFields.Map((Schema, Entity));
        //Assert
        record.Id.ShouldBe(Entity.Id);
        record.Field.ForEach(field =>
        {
            field.ShouldNotBeNull();
            Entity.Attributes.Keys.ShouldContain(field.Name);
        });
        record.Field.First(f => f.Name == "name").Value.ShouldBe("Test Account");
        var lookupField = record.Field.First(f => f.Name == "primarycontactid");
        record.Field.FirstOrDefault(f => f.Name == "revenue").ShouldBeNull();
        lookupField.Value.ShouldBe(Entity.GetAttributeValue<EntityReference>("primarycontactid").Id.ToString());
        lookupField.Lookupentity.ShouldBe("contact");
        lookupField.Lookupentityname.ShouldBeNull();
    }
    [Fact]
    public void GivenAnEntity_WhenItIsMappedToARecordWithEmptyFields_ThenItTheRecordShouldBeProperplyCreated()
    {
        //Arrange
        var Schema = FakeSchemas.Account;
        var Entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["primarycontactid"] = new EntityReference("contact", Guid.NewGuid())
        };
        //Act
        var record = _mapperWithEmptyFields.Map((Schema, Entity));
        //Assert
        record.Id.ShouldBe(Entity.Id);
        record.Field.First(f => f.Name == "name").Value.ShouldBe("Test Account");
        var lookupField = record.Field.First(f => f.Name == "primarycontactid");
        lookupField.Value.ShouldBe(Entity.GetAttributeValue<EntityReference>("primarycontactid").Id.ToString());
        lookupField.Lookupentity.ShouldBe("contact");
        lookupField.Lookupentityname.ShouldBeNull();
        var revenue = record.Field.First(f => f.Name == "revenue");
        revenue.Value.ShouldBeNull();
        revenue.IsNull.ShouldBeTrue();
    }
}

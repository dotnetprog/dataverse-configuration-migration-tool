using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Interceptors;
public abstract class BaseEntityReferenceFieldInterceptorTests<T> : BaseEntityInterceptorTests<T>
    where T : BaseEntityReferenceFieldInterceptor
{
    protected string EntityName { get; }
    protected string PrimaryKeyName { get; }
    protected BaseEntityReferenceFieldInterceptorTests(string EntityName, string PrimaryKeyName)
    {
        this.EntityName = EntityName;
        this.PrimaryKeyName = PrimaryKeyName;
    }
    protected abstract Task<Entity> GetEntityByIdAsync(Guid Id);
    protected abstract Task<Entity> GetEntityByNameAsync(string Name);
    [Fact]
    public async Task GivenAnEntityWithEntityReferenceField_WhenItsIntercepted_ThenItShouldResolveLookUpByName()
    {
        //Arrange
        var recordName = "Test Team";
        var expectedRecord = new Entity(EntityName)
        {
            Id = Guid.NewGuid(),
            ["name"] = recordName
        };
        GetEntityByNameAsync(recordName).Returns(expectedRecord);

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            [PrimaryKeyName] = new EntityReference(EntityName, Guid.NewGuid()) { Name = recordName }
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.GetAttributeValue<EntityReference>(PrimaryKeyName).Id.ShouldBe(expectedRecord.Id);
    }
    [Fact]
    public async Task GivenAnEntityWithEntityReferenceField_WhenItsIntercepted_ThenItShouldResolveLookUpById()
    {
        //Arrange
        var recordName = "Test team";
        var expectedRecord = new Entity(EntityName)
        {
            Id = Guid.NewGuid(),
            ["name"] = recordName
        };
        GetEntityByIdAsync(expectedRecord.Id).Returns(expectedRecord);

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            [PrimaryKeyName] = expectedRecord.ToEntityReference()
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.GetAttributeValue<EntityReference>(PrimaryKeyName).Id.ShouldBe(expectedRecord.Id);
    }
    [Fact]
    public async Task GivenAnEntityWithLookUpField_WhenItsInterceptedAndCantBeResolved_ThenItShouldRemoveFieldFromEntity()
    {
        //Arrange
        var recordName = "Test team";
        var expectedRecord = new Entity(EntityName)
        {
            Id = Guid.NewGuid(),
            ["name"] = recordName
        };

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            [PrimaryKeyName] = expectedRecord.ToEntityReference()
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.Attributes.FirstOrDefault(kv => kv.Key == PrimaryKeyName).ShouldBe(default);
    }
}

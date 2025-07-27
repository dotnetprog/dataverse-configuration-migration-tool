using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Interceptors;
public class TargetUserInterceptorTests : BaseEntityInterceptorTests<TargetUserInterceptor>
{
    private readonly ISystemUserRepository systemUserRepository = Substitute.For<ISystemUserRepository>();
    protected override TargetUserInterceptor CreateInterceptor() => new TargetUserInterceptor(systemUserRepository);


    [Fact]
    public async Task GivenAnEntityWithUserField_WhenItsIntercepted_ThenItShouldResolveUserByNameWithRepository()
    {
        //Arrange
        var userFullName = "Test user";
        var expectedUser = new Entity("systemuser")
        {
            Id = Guid.NewGuid(),
            ["fullname"] = userFullName
        };
        systemUserRepository.GetByName(userFullName).Returns(expectedUser);

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["ownerid"] = new EntityReference("systemuser", Guid.NewGuid()) { Name = userFullName }
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.GetAttributeValue<EntityReference>("ownerid").Id.ShouldBe(expectedUser.Id);
    }
    [Fact]
    public async Task GivenAnEntityWithUserField_WhenItsIntercepted_ThenItShouldResolveUserByIdWithRepository()
    {
        //Arrange
        var userFullname = "Test User";
        var expectedUser = new Entity("systemuser")
        {
            Id = Guid.NewGuid(),
            ["fullname"] = userFullname
        };
        systemUserRepository.GetByIdAsync(expectedUser.Id).Returns(expectedUser);

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["ownerid"] = expectedUser.ToEntityReference()
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.GetAttributeValue<EntityReference>("ownerid").Id.ShouldBe(expectedUser.Id);
    }
    [Fact]
    public async Task GivenAnEntityWithUserField_WhenItsInterceptedAndCantBeResolved_ThenItShouldRemoveFieldFromEntity()
    {
        //Arrange
        var userFullname = "Test User";
        var expectedUser = new Entity("systemuser")
        {
            Id = Guid.NewGuid(),
            ["fullname"] = userFullname
        };

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["ownerid"] = expectedUser.ToEntityReference()
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.Attributes.FirstOrDefault(kv => kv.Key == "ownerid").ShouldBe(default);
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Interceptors;
public class BusinessUnitInterceptorTests : BaseEntityInterceptorTests<BusinessUnitInterceptor>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = Substitute.For<IBusinessUnitRepository>();




    [Fact]
    public async Task GivenAnEntityWithBusinessUnitField_WhenItsIntercepted_ThenItShouldResolveBusinessUnitByNameWithRepository()
    {
        //Arrange
        var buName = "Test Business Unit";
        var expectedBu = new Entity("businessunit")
        {
            Id = Guid.NewGuid(),
            ["name"] = buName
        };
        _businessUnitRepository.GetByNameAsync(buName).Returns(expectedBu);

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["businessunitid"] = new EntityReference("businessunit", Guid.NewGuid()) { Name = buName }
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.GetAttributeValue<EntityReference>("businessunitid").Id.ShouldBe(expectedBu.Id);
    }
    [Fact]
    public async Task GivenAnEntityWithBusinessUnitField_WhenItsIntercepted_ThenItShouldResolveBusinessUnitByIdWithRepository()
    {
        //Arrange
        var buName = "Test Business Unit";
        var expectedBu = new Entity("businessunit")
        {
            Id = Guid.NewGuid(),
            ["name"] = buName
        };
        _businessUnitRepository.GetByIdAsync(expectedBu.Id).Returns(expectedBu);

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["businessunitid"] = expectedBu.ToEntityReference()
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.GetAttributeValue<EntityReference>("businessunitid").Id.ShouldBe(expectedBu.Id);
    }
    [Fact]
    public async Task GivenAnEntityWithBusinessUnitField_WhenItsInterceptedAndCantBeResolved_ThenItShouldRemoveFieldFromEntity()
    {
        //Arrange
        var buName = "Test Business Unit";
        var expectedBu = new Entity("businessunit")
        {
            Id = Guid.NewGuid(),
            ["name"] = buName
        };

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["businessunitid"] = expectedBu.ToEntityReference()
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.Attributes.FirstOrDefault(kv => kv.Key == "businessunitid").ShouldBe(default);
    }

    protected override BusinessUnitInterceptor CreateInterceptor() => new BusinessUnitInterceptor(_businessUnitRepository);

}

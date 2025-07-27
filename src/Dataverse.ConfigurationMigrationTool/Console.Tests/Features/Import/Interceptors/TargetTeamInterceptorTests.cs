using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Interceptors;
public class TargetTeamInterceptorTests : BaseEntityInterceptorTests<TargetTeamInterceptor>
{
    private readonly ITeamRepository _teamRepository = Substitute.For<ITeamRepository>();
    protected override TargetTeamInterceptor CreateInterceptor() => new TargetTeamInterceptor(_teamRepository);
    [Fact]
    public async Task GivenAnEntityWithTeamField_WhenItsIntercepted_ThenItShouldResolveTeamByNameWithRepository()
    {
        //Arrange
        var team = "Test Team";
        var expectedTeam = new Entity("team")
        {
            Id = Guid.NewGuid(),
            ["name"] = team
        };
        _teamRepository.GetByNameAsync(team).Returns(expectedTeam);

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["ownerid"] = new EntityReference("team", Guid.NewGuid()) { Name = team }
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.GetAttributeValue<EntityReference>("ownerid").Id.ShouldBe(expectedTeam.Id);
    }
    [Fact]
    public async Task GivenAnEntityWithTeamField_WhenItsIntercepted_ThenItShouldResolveTeamByIdWithRepository()
    {
        //Arrange
        var teamName = "Test team";
        var expectedTeam = new Entity("team")
        {
            Id = Guid.NewGuid(),
            ["name"] = teamName
        };
        _teamRepository.GetByIdAsync(expectedTeam.Id).Returns(expectedTeam);

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["ownerid"] = expectedTeam.ToEntityReference()
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.GetAttributeValue<EntityReference>("ownerid").Id.ShouldBe(expectedTeam.Id);
    }
    [Fact]
    public async Task GivenAnEntityWithTeamField_WhenItsInterceptedAndCantBeResolved_ThenItShouldRemoveFieldFromEntity()
    {
        //Arrange
        var teamName = "Test team";
        var expectedTeam = new Entity("team")
        {
            Id = Guid.NewGuid(),
            ["name"] = teamName
        };

        var entity = new Entity("account")
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["ownerid"] = expectedTeam.ToEntityReference()
        };

        //Act
        var result = await InterceptAsync(entity);
        //Assert
        result.Attributes.FirstOrDefault(kv => kv.Key == "ownerid").ShouldBe(default);
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Services.Dataverse;
public class DataverseTeamRepositoryTests
{
    private readonly IXrmFakedContext _fakedContext;
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    private readonly DataverseTeamRepository _repository;
    public DataverseTeamRepositoryTests()
    {
        // Arrange
        _fakedContext = MiddlewareBuilder.New()
            .SetLicense(FakeXrmEasyLicense.NonCommercial)
            .AddCrud()
            .UseCrud()
            .Build();
        _repository = new DataverseTeamRepository(_fakedContext.GetAsyncOrganizationService2(), _memoryCache);
    }
    [Fact]
    public async Task GivenATeamRepository_WhenItFetchesByName_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test Team";
        var cacheKey = $"team.GetByNameAsync.{recordname}";
        var team = new Entity("team")
        {
            Id = Guid.NewGuid(),
            ["name"] = recordname
        };
        _fakedContext.Initialize(team);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetByNameAsync(recordname);

        //Assert
        result.Id.ShouldBe(team.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == team.Id));

    }
    [Fact]
    public async Task GivenATeamRepository_WhenItFetchesById_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test Team";

        var team = new Entity("team")
        {
            Id = Guid.NewGuid(),
            ["name"] = recordname
        };
        var cacheKey = $"team.GetByIdAsync.{team.Id}";
        _fakedContext.Initialize(team);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetByIdAsync(team.Id);

        //Assert
        result.Id.ShouldBe(team.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == team.Id));

    }
}

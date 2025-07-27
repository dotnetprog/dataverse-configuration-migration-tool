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
public class DataverseUserRepositoryTests
{
    private readonly IXrmFakedContext _fakedContext;
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    private readonly DataverseUserRepository _repository;
    public DataverseUserRepositoryTests()
    {
        // Arrange
        _fakedContext = MiddlewareBuilder.New()
            .SetLicense(FakeXrmEasyLicense.NonCommercial)
            .AddCrud()
            .UseCrud()
            .Build();
        _repository = new DataverseUserRepository(_fakedContext.GetAsyncOrganizationService2(), _memoryCache);
    }
    [Fact]
    public async Task GivenATeamRepository_WhenItFetchesByName_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test User";
        var cacheKey = $"user.GetByName.{recordname}";
        var user = new Entity("systemuser")
        {
            Id = Guid.NewGuid(),
            ["fullname"] = recordname
        };
        _fakedContext.Initialize(user);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetByName(recordname);

        //Assert
        result.Id.ShouldBe(user.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == user.Id));

    }
    [Fact]
    public async Task GivenATeamRepository_WhenItFetchesById_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test User";

        var user = new Entity("systemuser")
        {
            Id = Guid.NewGuid(),
            ["fullname"] = recordname
        };
        var cacheKey = $"user.GetByIdAsync.{user.Id}";
        _fakedContext.Initialize(user);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetByIdAsync(user.Id);

        //Assert
        result.Id.ShouldBe(user.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == user.Id));

    }
}

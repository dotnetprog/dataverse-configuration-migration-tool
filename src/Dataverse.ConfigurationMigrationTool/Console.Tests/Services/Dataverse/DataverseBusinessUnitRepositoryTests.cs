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
public class DataverseBusinessUnitRepositoryTests
{
    private readonly IXrmFakedContext _fakedContext;
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    private readonly DataverseBusinessUnitRepository _repository;
    public DataverseBusinessUnitRepositoryTests()
    {
        // Arrange
        _fakedContext = MiddlewareBuilder.New()
            .SetLicense(FakeXrmEasyLicense.NonCommercial)
            .AddCrud()
            .UseCrud()
            .Build();
        _repository = new DataverseBusinessUnitRepository(_memoryCache, _fakedContext.GetAsyncOrganizationService2());
    }
    [Fact]
    public async Task GivenABusinessUnitRepository_WhenItFetchesByName_ThenItShouldCacheItsResult()
    {
        //Arrange
        var buName = "Test Business Unit";
        var cacheKey = $"bu.GetByNameAsync.{buName}";
        var bu = new Entity("businessunit")
        {
            Id = Guid.NewGuid(),
            ["name"] = buName
        };
        _fakedContext.Initialize(bu);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetByNameAsync(buName);

        //Assert
        result.Id.ShouldBe(bu.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == bu.Id));

    }
    [Fact]
    public async Task GivenABusinessUnitRepository_WhenItFetchesById_ThenItShouldCacheItsResult()
    {
        //Arrange
        var buName = "Test Business Unit";

        var bu = new Entity("businessunit")
        {
            Id = Guid.NewGuid(),
            ["name"] = buName
        };
        var cacheKey = $"bu.GetByIdAsync.{bu.Id}";
        _fakedContext.Initialize(bu);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetByIdAsync(bu.Id);

        //Assert
        result.Id.ShouldBe(bu.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == bu.Id));

    }
}

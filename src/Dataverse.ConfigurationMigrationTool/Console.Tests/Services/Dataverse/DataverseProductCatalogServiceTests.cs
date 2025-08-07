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
public class DataverseProductCatalogServiceTests
{
    private readonly IXrmFakedContext _fakedContext;
    private readonly IMemoryCache _memoryCache = Substitute.For<IMemoryCache>();
    private readonly DataverseProductCatalogService _repository;
    public DataverseProductCatalogServiceTests()
    {
        // Arrange
        _fakedContext = MiddlewareBuilder.New()
            .SetLicense(FakeXrmEasyLicense.NonCommercial)
            .AddCrud()
            .UseCrud()
            .Build();
        _repository = new DataverseProductCatalogService(_fakedContext.GetAsyncOrganizationService2(), _memoryCache);
    }
    [Fact]
    public async Task GivenAProductCatalogServicey_WhenItFetchesTransactionCurrencyById_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test Team";

        var record = new Entity("transactioncurrency")
        {
            Id = Guid.NewGuid(),
            ["currencyname"] = recordname
        };
        var cacheKey = $"product.GetTransacationCurrencyById.{record.Id}";
        _fakedContext.Initialize(record);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetTransacationCurrencyById(record.Id);

        //Assert
        result.Id.ShouldBe(record.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == record.Id));

    }
    [Fact]
    public async Task GivenAProductCatalogServicey_WhenItFetchesTransactionCurrencyByName_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test Team";
        var cacheKey = $"product.GetTransacationCurrencyByName.{recordname}";
        var record = new Entity("transactioncurrency")
        {
            Id = Guid.NewGuid(),
            ["currencyname"] = recordname
        };
        _fakedContext.Initialize(record);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetTransacationCurrencyByName(recordname);

        //Assert
        result.Id.ShouldBe(record.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == record.Id));

    }
    [Fact]
    public async Task GivenAProductCatalogServicey_WhenItFetchesUoMById_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test Team";

        var record = new Entity("uom")
        {
            Id = Guid.NewGuid(),
            ["name"] = recordname
        };
        var cacheKey = $"product.GetUoMById.{record.Id}";
        _fakedContext.Initialize(record);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetUoMById(record.Id);

        //Assert
        result.Id.ShouldBe(record.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == record.Id));

    }
    [Fact]
    public async Task GivenAProductCatalogServicey_WhenItFetchesUoMByName_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test Team";
        var cacheKey = $"product.GetUoMByName.{recordname}";
        var record = new Entity("uom")
        {
            Id = Guid.NewGuid(),
            ["name"] = recordname
        };
        _fakedContext.Initialize(record);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetUoMByName(recordname);

        //Assert
        result.Id.ShouldBe(record.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == record.Id));

    }
    [Fact]
    public async Task GivenAProductCatalogServicey_WhenItFetchesUoMScheduleById_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test Team";

        var record = new Entity("uomschedule")
        {
            Id = Guid.NewGuid(),
            ["name"] = recordname
        };
        var cacheKey = $"product.GetUoMScheduleById.{record.Id}";
        _fakedContext.Initialize(record);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetUoMScheduleById(record.Id);

        //Assert
        result.Id.ShouldBe(record.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == record.Id));

    }
    [Fact]
    public async Task GivenAProductCatalogServicey_WhenItFetchesUoMScheduleByName_ThenItShouldCacheItsResult()
    {
        //Arrange
        var recordname = "Test Team";
        var cacheKey = $"product.GetUoMScheduleByName.{recordname}";
        var record = new Entity("uomschedule")
        {
            Id = Guid.NewGuid(),
            ["name"] = recordname
        };
        _fakedContext.Initialize(record);
        var CacheEntry = Substitute.For<ICacheEntry>();
        _memoryCache.TryGetValue(cacheKey, out Arg.Any<object>()).Returns(false);
        _memoryCache.CreateEntry(cacheKey).Returns(CacheEntry);

        //Act

        var result = await _repository.GetUoMScheduleByName(recordname);

        //Assert
        result.Id.ShouldBe(record.Id);
        CacheEntry.Received().SetValue(Arg.Is<Entity>(e => e.Id == record.Id));

    }
}

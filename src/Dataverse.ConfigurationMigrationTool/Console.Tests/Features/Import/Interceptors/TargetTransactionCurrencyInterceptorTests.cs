using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Interceptors;
public class TargetTransactionCurrencyInterceptorTests : BaseEntityReferenceFieldInterceptorTests<TargetTransactionCurrencyInterceptor>
{
    private readonly IProductCatalogService _productCatalogService = Substitute.For<IProductCatalogService>();

    public TargetTransactionCurrencyInterceptorTests() : base("transactioncurrency", "transactioncurrencyid")
    {
    }

    protected override TargetTransactionCurrencyInterceptor CreateInterceptor() => new TargetTransactionCurrencyInterceptor(_productCatalogService);

    protected override Task<Entity> GetEntityByIdAsync(Guid Id)
    {
        return _productCatalogService.GetTransacationCurrencyById(Id);
    }

    protected override Task<Entity> GetEntityByNameAsync(string Name)
    {
        return _productCatalogService.GetTransacationCurrencyByName(Name);
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public class TargetTransactionCurrencyInterceptor : BaseEntityReferenceFieldInterceptor
{
    private readonly IProductCatalogService _productCatalogService;
    public TargetTransactionCurrencyInterceptor(IProductCatalogService productCatalogService) : base("transactioncurrency")
    {
        _productCatalogService = productCatalogService;
    }

    protected override async Task<Entity> GetEntityByIdAsync(Guid Id)
    {
        return await _productCatalogService.GetTransacationCurrencyById(Id);
    }

    protected override async Task<Entity> GetEntityByNameAsync(string Name)
    {
        return await _productCatalogService.GetTransacationCurrencyByName(Name);
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public class TargetUoMInterceptor : BaseEntityReferenceFieldInterceptor
{
    private readonly IProductCatalogService _productCatalogService;
    public TargetUoMInterceptor(IProductCatalogService productCatalogService) : base("uom")
    {
        _productCatalogService = productCatalogService;
    }

    protected override async Task<Entity> GetEntityByIdAsync(Guid Id)
    {
        return await _productCatalogService.GetUoMById(Id);
    }

    protected override async Task<Entity> GetEntityByNameAsync(string Name)
    {
        return await _productCatalogService.GetUoMByName(Name);
    }
}

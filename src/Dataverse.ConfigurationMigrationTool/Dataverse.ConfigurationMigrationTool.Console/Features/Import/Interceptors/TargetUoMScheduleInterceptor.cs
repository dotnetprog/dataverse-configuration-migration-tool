using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public class TargetUoMScheduleInterceptor : BaseEntityReferenceFieldInterceptor
{
    private readonly IProductCatalogService _productCatalogService;
    public TargetUoMScheduleInterceptor(IProductCatalogService productCatalogService) : base("uomschedule")
    {
        _productCatalogService = productCatalogService;
    }

    protected override async Task<Entity> GetEntityByIdAsync(Guid Id)
    {
        return await _productCatalogService.GetUoMScheduleById(Id);
    }

    protected override async Task<Entity> GetEntityByNameAsync(string Name)
    {
        return await _productCatalogService.GetUoMScheduleByName(Name);
    }
}

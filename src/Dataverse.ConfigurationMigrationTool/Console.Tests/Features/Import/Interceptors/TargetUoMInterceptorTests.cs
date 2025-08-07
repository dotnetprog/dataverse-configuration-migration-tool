using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Interceptors;
public class TargetUoMInterceptorTests : BaseEntityReferenceFieldInterceptorTests<TargetUoMInterceptor>
{
    private readonly IProductCatalogService _productCatalogService = Substitute.For<IProductCatalogService>();

    public TargetUoMInterceptorTests() : base("uom", "uomid")
    {
    }

    protected override TargetUoMInterceptor CreateInterceptor() => new TargetUoMInterceptor(_productCatalogService);

    protected override Task<Entity> GetEntityByIdAsync(Guid Id)
    {
        return _productCatalogService.GetUoMById(Id);
    }

    protected override Task<Entity> GetEntityByNameAsync(string Name)
    {
        return _productCatalogService.GetUoMByName(Name);
    }
}

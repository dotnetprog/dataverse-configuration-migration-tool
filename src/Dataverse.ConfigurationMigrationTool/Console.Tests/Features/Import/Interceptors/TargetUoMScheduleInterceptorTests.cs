using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Microsoft.Xrm.Sdk;
using NSubstitute;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Interceptors;
public class TargetUoMScheduleInterceptorTests : BaseEntityReferenceFieldInterceptorTests<TargetUoMScheduleInterceptor>
{
    private readonly IProductCatalogService _productCatalogService = Substitute.For<IProductCatalogService>();

    public TargetUoMScheduleInterceptorTests() : base("uomschedule", "uomscheduleid")
    {
    }

    protected override TargetUoMScheduleInterceptor CreateInterceptor() => new TargetUoMScheduleInterceptor(_productCatalogService);

    protected override Task<Entity> GetEntityByIdAsync(Guid Id)
    {
        return _productCatalogService.GetUoMScheduleById(Id);
    }

    protected override Task<Entity> GetEntityByNameAsync(string Name)
    {
        return _productCatalogService.GetUoMScheduleByName(Name);
    }
}

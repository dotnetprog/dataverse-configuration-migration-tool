using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Connection;

public static class DataverseServiceProviderExtensions
{
    public static IServiceCollection AddDataverseClient(this IServiceCollection services, ServiceLifetime clientLifetime = ServiceLifetime.Transient)
    {
        var serviceItem = new ServiceDescriptor(typeof(IOrganizationServiceAsync2),
            (sp) =>
            {
                var factory = sp.GetRequiredService<IDataverseClientFactory>();
                return factory.Create();
            }, clientLifetime);
        services.Add(serviceItem);
        return services.AddSingleton<IDataverseClientFactory, SdkDataverseServiceFactory>();
    }


}

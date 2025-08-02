using Dataverse.ConfigurationMigrationTool.Console.Features.Export.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddExportFeature(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddScoped<IDataExportService, DataExportService>()
            .Configure<ExportCommandOption>(Configuration);
        return services;
    }
}

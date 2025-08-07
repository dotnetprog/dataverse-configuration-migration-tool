using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddImportFeature(this IServiceCollection services, IConfiguration Configuration)
    {

        return services
            .AddScoped<IMainConverter, ReflectionMainConverter>(_ =>
            {
                var valueConverterTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract &&
              !t.IsInterface && t.BaseType != null && t.BaseType.IsConstructedGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseValueConverter<>)).ToList();
                return new ReflectionMainConverter(valueConverterTypes);
            })
            .AddScoped<IDataverseValueConverter, DataverseValueConverter>()

            .AddScoped<IImportTaskProcessorService, ImportTaskProcessorService>()
            .AddScoped<IEntityInterceptor>((sp) =>
            {

                return new EntityInterceptorChainBuilder(sp)
                .StartsWith<BusinessUnitInterceptor>()
                .ThenWith<TargetUserInterceptor>()
                .ThenWith<TargetTeamInterceptor>()
                .ThenWith<TargetTransactionCurrencyInterceptor>()
                .ThenWith<TargetUoMInterceptor>()
                .ThenWith<TargetUoMScheduleInterceptor>()
                .BuildChain();
            })
            .Configure<ImportCommandOptions>(Configuration);

    }
    public static T BuildService<T>(this IServiceProvider serviceProvider) where T : class
    {

        return ActivatorUtilities.CreateInstance<T>(serviceProvider);
    }

}

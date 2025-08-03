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
            .AddSingleton<IMainConverter, ReflectionMainConverter>(_ =>
            {
                var valueConverterTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract &&
              !t.IsInterface && t.BaseType != null && t.BaseType.IsConstructedGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseValueConverter<>)).ToList();
                return new ReflectionMainConverter(valueConverterTypes);
            })
            .AddSingleton<IDataverseValueConverter, DataverseValueConverter>()

            .AddSingleton<IImportTaskProcessorService, ImportTaskProcessorService>()
            .AddSingleton<IEntityInterceptor>((sp) =>
            {
                var BusinessUnitInterceptor = sp.BuildService<BusinessUnitInterceptor>();
                var UserInterceptor = sp.BuildService<TargetUserInterceptor>();
                var TeamInterceptor = sp.BuildService<TargetTeamInterceptor>();
                BusinessUnitInterceptor.SetSuccessor(UserInterceptor)
                    .SetSuccessor(TeamInterceptor);
                return BusinessUnitInterceptor;
            })
            .Configure<ImportCommandOptions>(Configuration);

    }
    public static T BuildService<T>(this IServiceProvider serviceProvider) where T : class
    {

        return ActivatorUtilities.CreateInstance<T>(serviceProvider);
    }

}

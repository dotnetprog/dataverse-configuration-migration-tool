using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.FieldSchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.RelationshipSchemas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddImportFeature(this IServiceCollection services, IConfiguration Configuration)
    {

        return services.RegisterFromReflection<IFieldSchemaValidationRule>()
            .RegisterFromReflection<IRelationshipSchemaValidationRule>()
            .AddSingleton<IMainConverter, ReflectionMainConverter>(_ =>
            {
                var valueConverterTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract &&
              !t.IsInterface && t.BaseType != null && t.BaseType.IsConstructedGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseValueConverter<>)).ToList();
                return new ReflectionMainConverter(valueConverterTypes);
            })
            .AddSingleton<IDataverseValueConverter, DataverseValueConverter>()
            .AddTransient<IValidator<DataSchema>, SchemaValidator>()
            .AddTransient<IValidator<EntitySchema>, EntitySchemaValidator>()
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
    public static IServiceCollection UseCommands(this IServiceCollection services, params string[] args)
    {
        services.AddSingleton<IOptions<CommandProcessorHostingServiceOptions>>(Options.Create(new CommandProcessorHostingServiceOptions() { CommandVerb = args[0] }));
        return services.AddHostedService<CommandProcessorHostingService>();
    }
    public static IServiceCollection RegisterFromReflection<T>(this IServiceCollection services)
    {
        var genericType = typeof(T);
        var classes = Assembly.GetCallingAssembly().GetTypes()
        .Where(type => genericType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
        foreach (var c in classes)
            services.AddTransient(genericType, c);
        return services;
    }
}

using Dataverse.ConfigurationMigrationTool.Console.Features.Export;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
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

namespace Dataverse.ConfigurationMigrationTool.Console.Features;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationMigrationTool(this IServiceCollection services, IConfiguration Configuration)
    {
        return services
            .AddSharedServices()
            .AddImportFeature(Configuration)
            .AddExportFeature(Configuration);
    }
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        return services.RegisterFromReflection<IFieldSchemaValidationRule>()
            .RegisterFromReflection<IRelationshipSchemaValidationRule>()
            .AddTransient<IValidator<DataSchema>, SchemaValidator>()
            .AddTransient<IValidator<EntitySchema>, EntitySchemaValidator>();
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

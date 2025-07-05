using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.FieldSchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules.EntitySchemas.RelationshipSchemas;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xrm.Sdk;
using System.Reflection;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddImportFeature(this IServiceCollection services)
        {

            return services.RegisterFromReflection<IFieldSchemaValidationRule>()
                .RegisterFromReflection<IRelationshipSchemaValidationRule>()
                .AddSingleton<IDataverseValueConverter, DataverseValueConverter>(_ =>
                {
                    var testtype = typeof(BaseValueConverter<EntityReference>);
                    var testtype2 = typeof(EntityReferenceValueConverter);
                    var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract &&
                    !t.IsInterface && t.BaseType != null && t.BaseType.IsConstructedGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseValueConverter<>)).ToList();
                    return new DataverseValueConverter(types);
                })
                .AddTransient<IValidator<ImportSchema>, SchemaValidator>()
                .AddTransient<IValidator<EntitySchema>, EntitySchemaValidator>()
                .AddSingleton<IImportTaskProcessorService, ImportTaskProcessorService>();
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
}

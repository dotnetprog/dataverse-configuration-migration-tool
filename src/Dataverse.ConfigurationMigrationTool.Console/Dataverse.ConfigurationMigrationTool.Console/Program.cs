using Cocona;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using System.Reflection;

var builder = CoconaApp.CreateBuilder();
builder.Configuration
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json", false, false)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", false, false);
if (!builder.Environment.IsProduction())
{
    //Secrets should never be in clear text in source controlled file such appsettings.json.
    //For Developement, we therefore store them locally into UserSecrets Store, part of dotnet foundation.
    //For Production, secrets can be either written into appsettings.Production.json file by pipeline
    //or you can configure another Configuration Provider to provide the secrets like AzureKeyvault or Hashicorp Vault.
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
}
builder.Services
    .AddLogging()
    .Configure<SdkDataverseServiceFactoryOptions>(builder.Configuration.GetSection("Dataverse"))
    .Configure<ParallelismBulkOrganizationServiceOptions>(builder.Configuration.GetSection("Dataverse"))
    .AddTransient<IImportDataProvider, FileReaderDataImportProvider>()
    .AddSingleton<IFileDataReader, XmlFileDataReader>()
    .AddTransient<IMetadataService, DataverseMetadataService>()
    .AddTransient<IValidator<ImportSchema>, SchemaValidator>()
    .AddSingleton<IDataverseValueConverter, DataverseValueConverter>((sp) =>
    {
        var testtype = typeof(BaseValueConverter<EntityReference>);
        var testtype2 = typeof(EntityReferenceValueConverter);
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsAbstract &&
        !t.IsInterface && t.BaseType != null && t.BaseType.IsConstructedGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseValueConverter<>)).ToList();
        return new DataverseValueConverter(types);
    })
    .AddTransient<ServiceClient>((sp) => (ServiceClient)sp.GetRequiredService<IDataverseClientFactory>().Create())
    .AddSingleton<IBulkOrganizationService, ParallelismBulkOrganizationService>()
    .AddSingleton<IImportTaskProcessorService, ImportTaskProcessorService>()
    .AddDataverseClient();

var app = builder.Build();
app.AddCommands<ImportCommands>();
await app.RunAsync();
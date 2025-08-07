using Dataverse.ConfigurationMigrationTool.Console.Features;
using Dataverse.ConfigurationMigrationTool.Console.Features.Export;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Connection;
using Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem;
using Dataverse.ConfigurationMigrationTool.Console.Services.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using System.Reflection;
var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureHostConfiguration((config) =>
{
    // Configure the host configuration, such as environment variables, command line arguments, etc.
    config.AddEnvironmentVariables().AddCommandLine(args);

});
builder.ConfigureAppConfiguration((context, config) =>
{
    config
        .AddEnvironmentVariables()
        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
        .AddJsonFile("appsettings.json", false, false)
        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", false, false);
    if (!context.HostingEnvironment.IsProduction())
    {
        //Secrets should never be in clear text in source controlled file such appsettings.json.
        //For Developement, we therefore store them locally into UserSecrets Store, part of dotnet foundation.
        //For Production, secrets can be either written into appsettings.Production.json file by pipeline
        //or you can configure another Configuration Provider to provide the secrets like AzureKeyvault or Hashicorp Vault.
        config.AddUserSecrets(Assembly.GetExecutingAssembly());
    }
    config.AddCommandLine(args);
    Console.WriteLine($"Using configuration file: appsettings.{context.HostingEnvironment.EnvironmentName}.json");
});
builder.ConfigureServices((context, services) =>
{

    services
    .AddLogging(lb => lb.AddConsole())
    .AddScoped<IDomainService, DataverseDomainService>()
    .Configure<SdkDataverseServiceFactoryOptions>(context.Configuration.GetSection("Dataverse"))
    .Configure<ParallelismBulkOrganizationServiceOptions>(context.Configuration.GetSection("Dataverse"))
    .AddScoped<IImportDataProvider, FileReaderDataImportProvider>()
    .AddSingleton<IFileDataService, XmlFileDataReader>()
    .AddScoped<IMetadataService, DataverseMetadataService>()
    .AddScoped<ServiceClient>((sp) => (ServiceClient)sp.GetRequiredService<IDataverseClientFactory>().Create())
    .AddScoped<IBulkOrganizationService, ParallelismBulkOrganizationService>()
    .AddDataverseClient(ServiceLifetime.Scoped)
    .AddConfigurationMigrationTool(context.Configuration)
    .UseCommands(args)
    .AddMemoryCache()
    .AddScoped<ISystemUserRepository, DataverseUserRepository>()
    .AddScoped<ITeamRepository, DataverseTeamRepository>()
    .AddScoped<IBusinessUnitRepository, DataverseBusinessUnitRepository>()
    .AddScoped<IProductCatalogService, DataverseProductCatalogService>();
    // Configure other services.
});

var app = builder.Build();
await app.RunAsync();

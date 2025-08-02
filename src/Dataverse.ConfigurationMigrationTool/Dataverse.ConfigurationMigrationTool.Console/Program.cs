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
        .AddCommandLine(args)
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
    Console.WriteLine($"Using configuration file: appsettings.{context.HostingEnvironment.EnvironmentName}.json");
});
builder.ConfigureServices((context, services) =>
{

    services
    .AddLogging(lb => lb.AddConsole())
    .AddScoped<IDomainService, DataverseDomainService>()
    .Configure<SdkDataverseServiceFactoryOptions>(context.Configuration.GetSection("Dataverse"))
    .Configure<ParallelismBulkOrganizationServiceOptions>(context.Configuration.GetSection("Dataverse"))
    .AddTransient<IImportDataProvider, FileReaderDataImportProvider>()
    .AddSingleton<IFileDataService, XmlFileDataReader>()
    .AddTransient<IMetadataService, DataverseMetadataService>()
    .AddTransient<ServiceClient>((sp) => (ServiceClient)sp.GetRequiredService<IDataverseClientFactory>().Create())
    .AddSingleton<IBulkOrganizationService, ParallelismBulkOrganizationService>()
    .AddDataverseClient()
    .AddConfigurationMigrationTool(context.Configuration)
    .UseCommands(args)
    .AddMemoryCache()
    .AddTransient<ISystemUserRepository, DataverseUserRepository>()
    .AddTransient<ITeamRepository, DataverseTeamRepository>()
    .AddTransient<IBusinessUnitRepository, DataverseBusinessUnitRepository>();
    // Configure other services.
});

var app = builder.Build();
await app.RunAsync();

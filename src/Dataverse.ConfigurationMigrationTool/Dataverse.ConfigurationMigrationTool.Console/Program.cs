using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem;
using Dataverse.ConfigurationMigrationTool.Console.Services.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using System.Reflection;
var builder = new HostBuilder();
builder.ConfigureHostConfiguration((config) =>
{
    // Configure the host configuration, such as environment variables, command line arguments, etc.
    config.AddEnvironmentVariables();
});
builder.ConfigureAppConfiguration((context, config) =>
{
    config
        .AddEnvironmentVariables()
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
    foreach (var arg in context.Configuration.AsEnumerable())
    {
        Console.WriteLine($"Configuration: {arg.Key} => {arg.Value}");
    }
});
builder.ConfigureServices((context, services) =>
{

    services
    .AddLogging(lb => lb.AddConsole())
    .Configure<SdkDataverseServiceFactoryOptions>(context.Configuration.GetSection("Dataverse"))
    .Configure<ParallelismBulkOrganizationServiceOptions>(context.Configuration.GetSection("Dataverse"))
    .AddTransient<IImportDataProvider, FileReaderDataImportProvider>()
    .AddSingleton<IFileDataReader, XmlFileDataReader>()
    .AddTransient<IMetadataService, DataverseMetadataService>()
    .AddTransient<ServiceClient>((sp) => (ServiceClient)sp.GetRequiredService<IDataverseClientFactory>().Create())
    .AddSingleton<IBulkOrganizationService, ParallelismBulkOrganizationService>()
    .AddDataverseClient()
    .AddImportFeature();
    // Configure other services.
});
builder.ConfigureCocona(args, configureApplication: app =>
{
    // Configure your app's commands normally as you would with app
    app.UseImportFeature();
});
await builder.RunConsoleAsync(x => x.SuppressStatusMessages = true);
//var builder = CoconaApp.CreateBuilder();
//builder.Configuration
//        .AddEnvironmentVariables()
//        .AddJsonFile("appsettings.json", false, false)
//        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", false, false);
//Console.WriteLine($"Using configuration file: appsettings.{builder.Environment.EnvironmentName}.json");
//foreach (var arg in args)
//{
//    Console.WriteLine($"Argument: {arg}");
//}
//if (!builder.Environment.IsProduction())
//{
//    //Secrets should never be in clear text in source controlled file such appsettings.json.
//    //For Developement, we therefore store them locally into UserSecrets Store, part of dotnet foundation.
//    //For Production, secrets can be either written into appsettings.Production.json file by pipeline
//    //or you can configure another Configuration Provider to provide the secrets like AzureKeyvault or Hashicorp Vault.
//    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
//}
//builder.Services
//    .AddLogging(lb => lb.AddConsole())
//    .Configure<SdkDataverseServiceFactoryOptions>(builder.Configuration.GetSection("Dataverse"))
//    .Configure<ParallelismBulkOrganizationServiceOptions>(builder.Configuration.GetSection("Dataverse"))
//    .AddTransient<IImportDataProvider, FileReaderDataImportProvider>()
//    .AddSingleton<IFileDataReader, XmlFileDataReader>()
//    .AddTransient<IMetadataService, DataverseMetadataService>()
//    .AddTransient<ServiceClient>((sp) => (ServiceClient)sp.GetRequiredService<IDataverseClientFactory>().Create())
//    .AddSingleton<IBulkOrganizationService, ParallelismBulkOrganizationService>()
//    .AddDataverseClient()
//    .AddImportFeature();
//Console.WriteLine($"Services are completed");
//var app = builder.Build();
//app.UseImportFeature();
//Console.WriteLine($"Running App");
//await app.RunAsync();
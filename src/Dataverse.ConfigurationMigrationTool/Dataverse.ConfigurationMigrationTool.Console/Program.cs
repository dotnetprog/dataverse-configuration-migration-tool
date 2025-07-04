﻿using Cocona;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem;
using Dataverse.ConfigurationMigrationTool.Console.Services.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.PowerPlatform.Dataverse.Client;
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
    .AddTransient<ServiceClient>((sp) => (ServiceClient)sp.GetRequiredService<IDataverseClientFactory>().Create())
    .AddSingleton<IBulkOrganizationService, ParallelismBulkOrganizationService>()
    .AddDataverseClient()
    .AddImportFeature();

var app = builder.Build();
app.UseImportFeature();
await app.RunAsync();
![.Net](https://img.shields.io/badge/.NET_8_SDK-5C2D91?style=for-the-badge&logoColor=white) [![Main Workflow](https://github.com/dotnetprog/dataverse-configuration-migration-tool/actions/workflows/main-pipeline.yml/badge.svg)](https://github.com/dotnetprog/dataverse-configuration-migration-tool/actions/workflows/main-pipeline.yml) ![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/dotnetprog/aa1b559b3f614ea0719286f9e2972219/raw/code-coverage.json) 
# Dataverse Configuration Migration Tool

This repository contains a custom .NET CLI tool designed to export and import configuration data into Microsoft Dataverse environments. It streamlines the migration of configuration data, supports schema validation, and offers extensibility for advanced scenarios.
### Download latest release
Get latest version of the tool built on this [release](https://github.com/dotnetprog/dataverse-configuration-migration-tool/releases/latest)
> [!NOTE]  
> If you want to use the built version of the tool , `appsettings.Production.json` will need to be setup manually with your azure service principal credentials.
> [Quick Guide](https://recursion.no/blogs/dataverse-setup-service-principal-access-for-environment/) to create an azure service principal
## Why ❓

Configuration Migration Tool and the PowerPlatform Cli Tool (pac data import verb) seem to have it's limitations when automating in ci/cd. Also, these two only works on a windows machine. \
This new tool enables you to:

- customize/extend the tool to your needs
- use in CD pipeline easily
- runs on windows and **linux**

## ⭐Features⭐

:heavy_check_mark: Import configuration data into Dataverse \
:heavy_check_mark: Export configuration data from Dataverse \
:heavy_check_mark: Schema validation and rule-based checks \
:heavy_check_mark: Support for special characters and self-hierarchy imports \
:heavy_check_mark: Extensible for custom validation and reporting \
:heavy_check_mark: Many to Many Relationships supported \
:heavy_check_mark: Owner,Team And Business Unit field is now supported. If by Unique Id is not found in target env, it matches by name otherwise it ignores the field. Caching is used to avoid unnecessary calls to dataverse. \
:heavy_check_mark: Supports schemas and data files generated from :
- [PowerPlatform CLI Export](https://learn.microsoft.com/en-us/power-platform/developer/cli/reference/data#pac-data-export)
- [Configuration Migration tool](https://learn.microsoft.com/en-us/power-platform/admin/create-schema-export-configuration-data)


❌Field types not supported:
- MultiSelectOptionsets
- Image
- File




## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Access to a Microsoft Dataverse environment

### 🛠️Building the Tool🛠️
1. Clone the repository:
   ```powershell
   git clone https://github.com/dotnetprog/dataverse-configuration-migration-tool.git
   cd dataverse-configuration-migration-tool/src/Dataverse.ConfigurationMigrationTool/Dataverse.ConfigurationMigrationTool.Console
   ```
2. Build the project:
   ```powershell
   dotnet build
   ```

### Usage


Before running the tool, set your `clientId`, `clientSecret` and `url` securely using [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

```powershell
cd src/Dataverse.ConfigurationMigrationTool/Dataverse.ConfigurationMigrationTool.Console
dotnet user-secrets set "Dataverse:ClientId" "<your-client-id>"
dotnet user-secrets set "Dataverse:ClientSecret" "<your-client-secret>"
dotnet user-secrets set "Dataverse:Url" "<your-env-url>"
```

Run the CLI tool with the required arguments (no need to pass clientId or clientSecret on the command line):
#### example
```powershell
dotnet run --environment DOTNET_ENVIRONMENT=Development --project Dataverse.ConfigurationMigrationTool.Console -- import --data "path/to/data.xml" --schema "path/to/schema.xml"
```

#### 💻 Command Line Arguments 💻

Verb: `import`
- `--data` : Path to the data xml file, you can use `export-data` command or the microsoft tool (see last section).
- `--schema` : Path to the schema XML file

Verb: `export-data`
- `--schema` : Path to the schema XML file
- `--output` : output file path to save the exported data. This file can be used for the `import` command.

## 🤝 Contributing 🤝

Contributions are welcome! To get started:

1. Fork the repository and create a new branch for your feature or bugfix.
2. Make your changes and add tests as appropriate.
3. Ensure all tests pass:
   ```powershell
   dotnet test ../Console.Tests/Dataverse.ConfigurationMigrationTool.Console.Tests.csproj
   ```
4. Submit a pull request with a clear description of your changes.

Please review open issues and the todo list below for ideas on what to contribute.


## 🚀Continuous Delivery🚀

This repository includes example GitHub Actions workflows for building, testing, and deploying the tool:

- [`main-pipeline.yml`](.github/workflows/main-pipeline.yml): Handles build, test, artifact publishing, and triggers deployment workflows.
- [`cd-pipeline.yml`](.github/workflows/cd-pipeline.yml): Example deployment workflow for importing configuration data into a Dataverse environment using published artifacts and environment secrets.

You can use these workflows as a starting point for your own CI/CD automation. See the workflow files for details on environment variables, secrets, and deployment steps.


### </> Generating Schema and Data Files </>

To use the `import` or `export-data` command, you need a schema file and/or a data file. These can be generated from your Dataverse environment using the official Configuration Migration tool. For detailed instructions, refer to the Microsoft documentation:

- [Create a schema and export configuration data](https://learn.microsoft.com/en-us/power-platform/admin/create-schema-export-configuration-data)

This guide explains how to:
- Create a schema file that defines the data to export
- Export configuration data from your environment

> [!NOTE]  
> the `data_schema.xml` and `data.xml` will be inside the exported zip. You need to extract those files and use them to import command as described above.




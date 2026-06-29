# Dataverse Configuration Migration Tool CLI

This package is distributed as a .NET tool and exposes the command name `dcm`.

## Install

Install from NuGet:

```bash
dotnet tool install --global Dataverse.ConfigurationMigrationTool.Console
```

Install from a local package:

```bash
dotnet tool install --global Dataverse.ConfigurationMigrationTool.Console --add-source .\\nupkg
```

Update an existing installation:

```bash
dotnet tool update --global Dataverse.ConfigurationMigrationTool.Console
```

## Commands

The CLI supports these verbs:

- `dcm export-data`
- `dcm import`

## Authentication and configuration

The tool loads configuration from:

1. Environment variables
2. `appsettings.json`
3. `appsettings.{Environment}.json`
4. User secrets when running outside Production
5. Command-line arguments

### Dataverse settings

You can provide Dataverse connection settings with environment variables:

- `Dataverse__Url`
- `Dataverse__ClientId`
- `Dataverse__ClientSecret`

Example in PowerShell:

```powershell
$env:Dataverse__Url = "https://yourorg.crm.dynamics.com"
$env:Dataverse__ClientId = "00000000-0000-0000-0000-000000000000"
$env:Dataverse__ClientSecret = "your-client-secret"
```

You can also pass the same values on the command line:

```bash
dcm export-data --schema .\\schema.xml --output .\\data.xml --Dataverse:Url https://yourorg.crm.dynamics.com --Dataverse:ClientId 00000000-0000-0000-0000-000000000000 --Dataverse:ClientSecret your-client-secret
```

### Interactive login

For interactive authentication, use either of these switches:

- `-il`
- `--il`

Example:

```bash
dcm export-data --schema .\\schema.xml --output .\\data.xml --Dataverse:Url https://yourorg.crm.dynamics.com --il
```

When interactive login is enabled, the import command may prompt more than once because the tool creates multiple Dataverse connections.

## Export data

Exports Dataverse data using a schema file and writes the result to an output file.

### Required options

- `--schema` - Path to the schema XML file
- `--output` - Path to the exported output file

### Example

```bash
dcm export-data --schema .\\schema.xml --output .\\exporteddata.xml
```

### Example with interactive login

```bash
dcm export-data --schema .\\schema.xml --output .\\exporteddata.xml --Dataverse:Url https://yourorg.crm.dynamics.com --il
```

## Import data

Imports Dataverse data from a data file using the provided schema.

### Required options

- `--schema` - Path to the schema XML file
- `--data` - Path to the data file to import

### Example

```bash
dcm import --schema .\\schema.xml --data .\\data.zip
```

### Example with interactive login

```bash
dcm import --schema .\\schema.xml --data .\\data.zip --Dataverse:Url https://yourorg.crm.dynamics.com --il
```

## Optional flags

- `--enable-empty-fields` - Enables empty field handling during execution

Example:

```bash
dcm import --schema .\\schema.xml --data .\\data.zip --enable-empty-fields
```

## Environment selection

The active settings file depends on the .NET environment. For example:

```powershell
$env:DOTNET_ENVIRONMENT = "Development"
```

With this value, the tool loads `appsettings.Development.json` in addition to `appsettings.json`.

## Exit behavior

If a command fails, the process exits with code `-100`.

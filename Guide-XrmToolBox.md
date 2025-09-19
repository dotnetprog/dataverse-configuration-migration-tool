# Dataverse Configuration Migration Tool For XrmToolBox

This tool is a plugin for [XrmToolBox](https://www.xrmtoolbox.com/) which allows you to create a schema definition file for data export and data import between dataverse environments.

This tool generates the schema definition file in the same format as the official tool from Microsoft: [Configuration Migration Tool](https://learn.microsoft.com/en-us/power-platform/alm/configure-and-deploy-tools)

This tool was created to support more data types that the official tool does.
## ⭐Key Features⭐
✔️ Schema definition file for export/import 

### Upcoming features 🔜
🔜 Supports for multiselect optionset \
🔜 ~~Configuration Data Importation~~ $${\color{red}This \space is \space no \space longer \space planned.}$$ \
🔜 ~~Configuration Data Exportation~~  <span style="color:red">This is no longer planned.</span>

> [!IMPORTANT]  
> Data import/export features will only be available through the cli tool.  \
> Since the cli tool is made with .Net Core and XTB Plugins is in .Net Framework, It's really hard to keep a sharable codebase and retricts the usage of some modern libraries.  \
> CLI tool documentation [here](https://github.com/dotnetprog/dataverse-configuration-migration-tool)


##  What's a schema definition file exactly 🤔❓

A Schema definition file is an xml file that contains the definition of entities which contains the fields and the many to many relationships that you'd like to export/import between environments.

## How to use the tool ?

### Prerequisites
- [.Net Framework 4.8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- [XrmToolBox](https://www.xrmtoolbox.com/)
    - version must be atleast 1.2025.7.71

### Getting Started

> Make sure you have installed this package from the tool library in XrmToolBox: `Dataverse.ConfigurationMigrationTool.XrmToolBox`

1. Open XrmToolBox
2. Install `Dataverse.ConfigurationMigrationTool.XrmToolBox` from Tool Library if needed
3. Open Tool : `Dataverse.ConfigurationMigrationTool.XrmToolBox`
4. Connect to an environment

### Create your schema file

![tutorial](https://raw.githubusercontent.com/dotnetprog/dataverse-configuration-migration-tool/main/images/ToolTutorial.png "tutorial")

1. Load your solutions
2. Select a solution
3. Select an entity
4. Select the fields and or relationships you want in your schema
5. Click on the right arrow
6. Click on the `Generate Schema File` and choose the location you want to save the file.

> **Note:** You can add more fields and relations from other entities before generating your schema file.

## Disclaimer

Although this tool has the same name as the official configuration migration tool provided by Microsoft, it is not an official tool.

It's a community homemade tool to offer an alternative with more features.




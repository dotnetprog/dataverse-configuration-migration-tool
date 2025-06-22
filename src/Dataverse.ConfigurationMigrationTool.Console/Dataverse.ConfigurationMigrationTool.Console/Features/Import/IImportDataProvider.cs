using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import
{
    public interface IImportDataProvider
    {
        Task<Entities> ReadFromFile(string filePath);
        Task<ImportSchema> ReadSchemaFromFile(string filePath);
    }
}

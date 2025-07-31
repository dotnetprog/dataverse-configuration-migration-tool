using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import
{
    public interface IImportDataProvider
    {
        Task<Entities> ReadFromFile(string filePath);
        Task<DataSchema> ReadSchemaFromFile(string filePath);
    }
}

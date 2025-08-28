namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain.Abstraction
{
    public interface IFileService
    {
        void WriteToFile<T>(string filePath, T data);
    }
}

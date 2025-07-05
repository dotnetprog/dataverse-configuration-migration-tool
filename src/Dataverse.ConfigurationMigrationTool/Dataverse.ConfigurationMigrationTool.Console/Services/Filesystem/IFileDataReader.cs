namespace Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem
{
    public interface IFileDataReader
    {
        Task<T> ReadAsync<T>(string path);
    }
}

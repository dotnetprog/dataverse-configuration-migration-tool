namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared
{
    public interface IFileDataReader
    {
        Task<T> ReadAsync<T>(string path);
    }
}

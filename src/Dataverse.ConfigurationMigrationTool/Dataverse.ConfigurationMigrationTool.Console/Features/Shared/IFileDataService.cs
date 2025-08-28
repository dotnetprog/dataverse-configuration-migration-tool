using System.Threading.Tasks;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared
{
    public interface IFileDataService
    {
        Task<T> ReadAsync<T>(string path);
        Task WriteAsync<T>(T obj, string path);
    }
}



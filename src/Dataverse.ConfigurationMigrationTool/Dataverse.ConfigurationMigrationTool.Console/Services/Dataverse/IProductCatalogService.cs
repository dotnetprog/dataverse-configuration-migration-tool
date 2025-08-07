using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
public interface IProductCatalogService
{
    Task<Entity> GetTransacationCurrencyById(Guid Id);
    Task<Entity> GetTransacationCurrencyByName(string name);
    Task<Entity> GetUoMById(Guid Id);
    Task<Entity> GetUoMByName(string name);
    Task<Entity> GetUoMScheduleById(Guid Id);
    Task<Entity> GetUoMScheduleByName(string name);
}

using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public abstract class BaseEntityInterceptor : IEntityInterceptor
{
    private IEntityInterceptor Successor { get; set; }
    public async virtual Task<Entity> InterceptAsync(Entity entity)
    {
        if (Successor == null)
        {
            return entity;
        }
        return await Successor.InterceptAsync(entity);
    }

    public IEntityInterceptor SetSuccessor(IEntityInterceptor successor)
    {
        this.Successor = successor;
        return this.Successor;
    }
}

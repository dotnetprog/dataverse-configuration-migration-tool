using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public interface IEntityInterceptor
{
    /// <summary>
    /// Intercepts the entity before it is processed.
    /// </summary>
    /// <param name="entity">The entity to intercept.</param>
    /// <returns>The intercepted entity.</returns>
    Task<Entity> InterceptAsync(Entity entity);
    /// <summary>
    /// Intercepts the entity after it has been processed.
    /// </summary>
    /// <param name="entity">The entity to intercept.</param>
    /// <returns>The intercepted entity.</returns>
    IEntityInterceptor SetSuccessor(IEntityInterceptor successor);
}

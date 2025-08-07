namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public class EntityInterceptorChainBuilder
{
    private readonly IServiceProvider _serviceProvider;
    private IEntityInterceptor Root { get; set; }
    private IEntityInterceptor CurrentSuccessor { get; set; }
    public EntityInterceptorChainBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public EntityInterceptorChainBuilder StartsWith<TInterceptor>() where TInterceptor : class, IEntityInterceptor
    {
        var interceptor = _serviceProvider.BuildService<TInterceptor>();

        Root = interceptor;
        CurrentSuccessor = interceptor;
        return this;
    }
    public EntityInterceptorChainBuilder ThenWith<TInterceptor>() where TInterceptor : class, IEntityInterceptor
    {
        CurrentSuccessor = CurrentSuccessor.SetSuccessor(_serviceProvider.BuildService<TInterceptor>());
        return this;
    }
    public IEntityInterceptor BuildChain() => Root;

}

using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests;
public abstract class BaseMapperTests<TMapper, TSource, TDestination> where TMapper : IMapper<TSource, TDestination>
{

    private object[] _additionalParameters;
    protected BaseMapperTests(object[] additionalParameters = null)
    {
        _additionalParameters = additionalParameters ?? Array.Empty<object>();
    }
    private IMapper<TSource, TDestination> _mapper => CreateMapper();
    private TMapper CreateMapper() => (TMapper)Activator.CreateInstance(typeof(TMapper), _additionalParameters);

    protected TDestination RunTest(TSource source) => _mapper.Map(source);

}

using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Interceptors;
public abstract class BaseEntityInterceptorTests<T>
    where T : IEntityInterceptor
{
    private IEntityInterceptor Interceptor { get; set; }
    protected readonly IEntityInterceptor Successor = Substitute.For<IEntityInterceptor>();
    protected BaseEntityInterceptorTests()
    {
        Interceptor = CreateInterceptor();
        Interceptor.SetSuccessor(Successor);
        Successor.InterceptAsync(Arg.Any<Entity>())
            .Returns(x => x.Arg<Entity>());
    }
    protected abstract T CreateInterceptor();
    protected async Task<Entity> InterceptAsync(Entity entity, bool ShouldSuccessorBeCalled = true)
    {
        var result = await Interceptor.InterceptAsync(entity);
        if (ShouldSuccessorBeCalled)
        {
            await Successor.Received(1).InterceptAsync(entity);
        }
        else
        {
            await Successor.DidNotReceive().InterceptAsync(entity);
        }
        return result;
    }
    [Fact]
    public async Task GivenAnEntityInterceptorWith_NoSuccessor_ThenItShouldReturnEntity()
    {
        //Arrange
        var interceptor = CreateInterceptor();

        var entity = new Entity();
        //Act
        var result = await interceptor.InterceptAsync(entity);
        //Assert
        result.ShouldBe(entity);
        await Successor.DidNotReceive().InterceptAsync(entity);
    }
}

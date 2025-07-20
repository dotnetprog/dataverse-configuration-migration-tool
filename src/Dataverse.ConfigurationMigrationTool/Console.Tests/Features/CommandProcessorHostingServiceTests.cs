using Dataverse.ConfigurationMigrationTool.Console.Features;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Tests.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using System.Reflection;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features;
public class CommandProcessorHostingServiceTests
{
    private readonly IOptions<CommandProcessorHostingServiceOptions> _options;
    private readonly IServiceScopeFactory _serviceProviderFactory;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IServiceScope _serviceScope;
    private readonly IServiceCollection ServiceCollection = new ServiceCollection();
    private readonly ILogger<FakeCommand> commandLogger = Substitute.For<ILogger<FakeCommand>>();
    private readonly CommandProcessorHostingServiceOptions HostingOptions = new CommandProcessorHostingServiceOptions
    {
        CommandVerb = "fake"
    };

    public CommandProcessorHostingServiceTests()
    {

        ServiceCollection.AddSingleton<ILogger<FakeCommand>>(commandLogger);
        _serviceScope = Substitute.For<IServiceScope>();
        _serviceScope.ServiceProvider.Returns(ServiceCollection.BuildServiceProvider());
        _options = Substitute.For<IOptions<CommandProcessorHostingServiceOptions>>();
        _options.Value.Returns(HostingOptions);
        _serviceProviderFactory = Substitute.For<IServiceScopeFactory>();
        _serviceProviderFactory.CreateScope().Returns(_serviceScope);
        _lifetime = Substitute.For<IHostApplicationLifetime>();

    }

    [Fact]
    public async Task GivenACommandVerb_WhenCommandProcessorRuns_ThenItShouldExecuteTheProperCommand()
    {
        //Arrange

        var host = new CommandProcessorHostingService(_options, _serviceProviderFactory, _lifetime, Assembly.GetExecutingAssembly());
        //Act
        await host.StartAsync(CancellationToken.None);

        //Assert
        commandLogger.ShouldHaveLogged(LogLevel.Information, "Fake command executed successfully.");

    }

    [Fact]
    public async Task GivenAnNonExistantCommandVerb_WhenCommandProcessorRuns_ThenItShouldExecuteTheProperCommand()
    {
        //Arrange
        var host = new CommandProcessorHostingService(_options, _serviceProviderFactory, _lifetime);
        //Act
        Func<Task> act = () => host.StartAsync(CancellationToken.None);

        //Assert
        var ex = await act.ShouldThrowAsync<InvalidOperationException>();
        ex.Message.ShouldBe($"No command found for verb '{HostingOptions.CommandVerb}'");

    }



}
[CommandVerb("fake")]
public class FakeCommand : ICommand
{
    private readonly ILogger<FakeCommand> _logger;

    public FakeCommand(ILogger<FakeCommand> logger)
    {
        _logger = logger;
    }

    public Task Execute()
    {
        _logger.LogInformation("Fake command executed successfully.");
        return Task.CompletedTask;
    }
}

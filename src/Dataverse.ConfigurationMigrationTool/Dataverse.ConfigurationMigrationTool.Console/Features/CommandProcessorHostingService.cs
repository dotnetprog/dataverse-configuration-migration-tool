using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Dataverse.ConfigurationMigrationTool.Console.Features;
public class CommandProcessorHostingService : BackgroundService
{
    private readonly CommandProcessorHostingServiceOptions _options;
    private readonly IServiceScopeFactory _serviceProviderFactory;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IEnumerable<Assembly> _commandAssemblies;

    public CommandProcessorHostingService(IOptions<CommandProcessorHostingServiceOptions> options, IServiceScopeFactory serviceProviderFactory, IHostApplicationLifetime lifetime, params Assembly[] commandAssemblies)
    {
        _options = options.Value;
        _serviceProviderFactory = serviceProviderFactory;
        _lifetime = lifetime;
        _commandAssemblies = commandAssemblies.Length > 0 ? commandAssemblies : new[] { Assembly.GetExecutingAssembly() };
    }
    public CommandProcessorHostingService(IOptions<CommandProcessorHostingServiceOptions> options, IServiceScopeFactory serviceProviderFactory, IHostApplicationLifetime lifetime) : this(options, serviceProviderFactory, lifetime, [])
    {

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var types = from type in _commandAssemblies.SelectMany(a => a.GetTypes())
                    where Attribute.IsDefined(type, typeof(CommandVerbAttribute)) &&
                          type.IsClass &&
                          !type.IsAbstract &&
                          typeof(ICommand).IsAssignableFrom(type)
                    select (type, (CommandVerbAttribute)Attribute.GetCustomAttribute(type, typeof(CommandVerbAttribute)));

        var mathingVerbCommandType = types.FirstOrDefault(t => t.Item2.Verb.Equals(_options.CommandVerb, StringComparison.OrdinalIgnoreCase)).type;
        using (var scope = _serviceProviderFactory.CreateScope())
        {
            if (mathingVerbCommandType == null)
            {
                throw new InvalidOperationException($"No command found for verb '{_options.CommandVerb}'");
            }
            var command = ActivatorUtilities.CreateInstance(scope.ServiceProvider, mathingVerbCommandType) as ICommand;
            await command.Execute();


        }
        _lifetime.StopApplication();
    }
}

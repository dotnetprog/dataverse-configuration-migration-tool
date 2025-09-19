using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;

namespace Dataverse.ConfigurationMigrationTool.Console.ConfigurationProviders;
public class CustomCommandLineConfigurationSource : IConfigurationSource
{
    public IDictionary<string, string>? SwitchMappings { get; set; }
    public IEnumerable<string>? FlagMappings { get; set; }
    /// <summary>
    /// Gets or sets the command line arguments.
    /// </summary>
    public IEnumerable<string> Args { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Builds the <see cref="CommandLineConfigurationProvider"/> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
    /// <returns>A <see cref="CommandLineConfigurationProvider"/>.</returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new CustomCommandLineConfigurationProvider(Args, SwitchMappings, FlagMappings);
    }
}

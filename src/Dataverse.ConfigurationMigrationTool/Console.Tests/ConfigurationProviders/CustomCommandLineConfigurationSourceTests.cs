using Dataverse.ConfigurationMigrationTool.Console.ConfigurationProviders;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.ConfigurationProviders;
public class CustomCommandLineConfigurationSourceTests
{
    private CustomCommandLineConfigurationSource ConfigSource { get; }
    public CustomCommandLineConfigurationSourceTests()
    {
        ConfigSource = new CustomCommandLineConfigurationSource()
        {
            Args = [],
        };
    }

    [Fact]
    public void GivenACommandLineSource_WhenItBuildsAConfigProvider_ThenACommandLineConfigurationProviderIsReturned()
    {
        // Act
        var provider = ConfigSource.Build(null);
        // Assert
        provider.ShouldBeOfType<CustomCommandLineConfigurationProvider>();
    }

}

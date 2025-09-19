using Dataverse.ConfigurationMigrationTool.Console.ConfigurationProviders;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.ConfigurationProviders;
public class CustomCommandLineConfigurationExtensionsTests
{
    private readonly IConfigurationBuilder _builder = Substitute.For<IConfigurationBuilder>();
    public CustomCommandLineConfigurationExtensionsTests()
    {
        _builder.Add(Arg.Any<IConfigurationSource>()).Returns(_builder);
    }
    [Fact]
    public void GivenAConfigurationBuilder_WhenItAddsCustomCommandline_ThenItAddsTheCustomCommandLineConfigurationProvider()
    {
        // Arrange
        var args = new[] { "--key=value" };
        // Act
        _builder.AddCustomCommandline(args);
        // Assert
        _builder.Received(1).Add(Arg.Is<CustomCommandLineConfigurationSource>(source =>
            source.Args.SequenceEqual(args) &&
            source.SwitchMappings == null &&
            source.FlagMappings == null));
    }
}

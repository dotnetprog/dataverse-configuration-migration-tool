using Cocona.Builder;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;
using NSubstitute;
using System.Reflection;
namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import;
public class CoconaAppExtensionsTests
{
    [Fact]
    public void GivenACoconaCommandBuilder_WhenImportFeatureIsUsed_ThenItShouldAddImportCommand()
    {
        // Arrange
        var commandBuilder = Substitute.For<ICoconaCommandsBuilder>();
        // Act
        commandBuilder.UseImportFeature();
        // Assert
        commandBuilder.Received(1).Add(Arg.Is<TypeCommandDataSource>(t => typeof(TypeCommandDataSource).GetField("_type", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(t) == typeof(ImportCommands)));

    }
}

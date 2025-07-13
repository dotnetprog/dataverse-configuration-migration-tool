using Cocona;
using Cocona.Builder;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import;

public static class CoconaAppExtensions
{
    public static ICoconaCommandsBuilder UseImportFeature(this ICoconaCommandsBuilder app)
    {
        app.AddCommands<ImportCommands>();
        return app;
    }
}

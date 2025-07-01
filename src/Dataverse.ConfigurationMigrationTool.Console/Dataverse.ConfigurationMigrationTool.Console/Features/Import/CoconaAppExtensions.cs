using Cocona;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import
{
    public static class CoconaAppExtensions
    {
        public static CoconaApp UseImportFeature(this CoconaApp app)
        {
            app.AddCommands<ImportCommands>();
            return app;
        }
    }
}

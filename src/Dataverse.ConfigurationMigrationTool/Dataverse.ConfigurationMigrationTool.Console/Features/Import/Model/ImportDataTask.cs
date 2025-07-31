using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model
{
    public class ImportDataTask
    {
        public EntitySchema EntitySchema { get; set; }
        public RelationshipSchema RelationshipSchema { get; set; }
        public string SouceEntityName { get; set; }

    }
}

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration
{
    public class ParallelismBulkOrganizationServiceOptions
    {
        public int MaxThreadCount { get; set; } = 5;
        public int MaxDegreeOfParallism { get; set; } = 5;
        public int BatchSize { get; set; } = 10;
    }
}

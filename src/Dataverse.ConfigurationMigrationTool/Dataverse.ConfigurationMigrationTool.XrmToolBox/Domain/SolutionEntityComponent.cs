namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain
{
    public class SolutionEntityComponent
    {
        public string EntityName { get; set; }
        public string DisplayEntityName { get; set; }
        public override string ToString()
        {
            return $"{DisplayEntityName} ({EntityName})";
        }
    }
}

using System;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Domain
{
    public class Solution
    {
        public Guid Id { get; set; }
        public string UniqueName { get; set; }
        public string FriendlyName { get; set; }
        public string Version { get; set; }
        public bool IsManaged { get; set; }

        public override string ToString()
        {
            return $"{FriendlyName} ({Version}) {(IsManaged ? "[Managed]" : "[Unmanaged]")}";
        }
    }
}

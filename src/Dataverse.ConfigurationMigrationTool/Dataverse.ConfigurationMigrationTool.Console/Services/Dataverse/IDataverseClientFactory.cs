using Microsoft.PowerPlatform.Dataverse.Client;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;

public interface IDataverseClientFactory
{

    IOrganizationServiceAsync2 Create();

}

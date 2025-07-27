using Microsoft.PowerPlatform.Dataverse.Client;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Connection;

public interface IDataverseClientFactory
{

    IOrganizationServiceAsync2 Create();

}

using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse
{
    public interface IBulkOrganizationService
    {
        Task<IEnumerable<OrganizationResponseFaultedResult>> ExecuteBulk(IEnumerable<OrganizationRequest> request);

    }
}

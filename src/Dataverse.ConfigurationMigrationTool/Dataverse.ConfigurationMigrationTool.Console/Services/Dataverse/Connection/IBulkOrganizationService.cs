using Dataverse.ConfigurationMigrationTool.Console.Common;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Connection
{
    public interface IBulkOrganizationService
    {
        Task<IEnumerable<OrganizationResponseFaultedResult>> ExecuteBulk(IEnumerable<OrganizationRequest> request, IEnumerable<string> faultToSkips = null);
        Task<IEnumerable<OrganizationResponseFaultedResult>> UpsertBulk(IEnumerable<UpsertRequest> requests);
        Task<Result<UpsertResponse, OrganizationResponseFaultedResult>> Upsert(UpsertRequest request);

    }
}

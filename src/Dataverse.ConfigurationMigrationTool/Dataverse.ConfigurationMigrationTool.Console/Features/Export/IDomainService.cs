using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export;
public interface IDomainService
{
    Task<IEnumerable<Record>> GetRecords(EntitySchema Schema);
    Task<IEnumerable<M2mrelationship>> GetM2mRelationships(ManyToManyRelationshipMetadata metadata);


}

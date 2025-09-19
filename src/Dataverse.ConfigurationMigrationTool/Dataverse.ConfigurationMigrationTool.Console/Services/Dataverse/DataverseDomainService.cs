using Dataverse.ConfigurationMigrationTool.Console.Common;
using Dataverse.ConfigurationMigrationTool.Console.Features.Export;
using Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
public class DataverseDomainService : IDomainService
{
    private readonly IOrganizationServiceAsync2 _orgService;
    private readonly ILogger<DataverseDomainService> _logger;
    private readonly DataverseDomainServiceOptions _options;
    private readonly IMapper<(EntitySchema, Entity), Record> _recordMapper;
    public DataverseDomainService(IOrganizationServiceAsync2 orgService, IOptions<DataverseDomainServiceOptions> options, ILogger<DataverseDomainService> logger)
    {
        this._options = options.Value;
        _orgService = orgService;
        _logger = logger;
        _recordMapper = new DataverseRecordToRecordMapper(this._options.AllowEmptyFields);
    }

    public async Task<IEnumerable<Record>> GetRecords(EntitySchema Schema)
    {
        var exportfields = Schema.Fields.Field.Select(f => f.Name).ToList();
        if (exportfields.Count == 0)
        {
            _logger.LogWarning("No fields specified for export in schema for entity {entityName}", Schema.Name);
            return Enumerable.Empty<Record>();
        }
        var query = new QueryExpression(Schema.Name)
        {
            ColumnSet = new ColumnSet(exportfields.ToArray()),

        };
        var entityCollection = await _orgService.RetrieveAll(query, page: 5000, _logger);

        var data = entityCollection.Entities.Select(e => _recordMapper.Map((Schema, e))).ToList();
        return data;
    }

    public async Task<IEnumerable<M2mrelationship>> GetM2mRelationships(ManyToManyRelationshipMetadata metadata)
    {
        var query = new QueryExpression(metadata.IntersectEntityName)
        {
            ColumnSet = new ColumnSet(true)
        };
        var entityCollection = await _orgService.RetrieveAll(query, page: 5000, _logger);

        return entityCollection.Entities.GroupBy(e => e.GetAttributeValue<Guid>(metadata.Entity1IntersectAttribute)).Select(g =>
        {
            return new M2mrelationship
            {
                Sourceid = g.Key,
                Targetentityname = metadata.Entity2LogicalName,
                Targetentitynameidfield = metadata.Entity2IntersectAttribute,
                M2mrelationshipname = metadata.IntersectEntityName,
                Targetids = new Targetids
                {
                    Targetid = g.Select(e => e.GetAttributeValue<Guid>(metadata.Entity2IntersectAttribute)).ToList()
                }
            };

        }).ToList();
    }
}

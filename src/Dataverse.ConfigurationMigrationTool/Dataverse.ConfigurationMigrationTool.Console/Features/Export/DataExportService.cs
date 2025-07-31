using Dataverse.ConfigurationMigrationTool.Console.Common;
using Dataverse.ConfigurationMigrationTool.Console.Features.Export.Mappers;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export;
public interface IDataExportService
{
    Task<TaskResult> ExportToFile(DataSchema Schema, string outputfile);
}
public class DataExportService : IDataExportService
{
    private readonly ILogger<DataExportService> _logger;
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync2;
    private readonly IMetadataService _metadataService;
    private static readonly IMapper<(EntityMetadata, EntitySchema, Entity), Record> _recordMapper = new DataverseRecordToRecordMapper();
    public DataExportService(ILogger<DataExportService> logger,
        IOrganizationServiceAsync2 organizationServiceAsync2,
        IMetadataService metadataService)
    {
        _logger = logger;
        _organizationServiceAsync2 = organizationServiceAsync2;
        _metadataService = metadataService;
    }

    public async Task<TaskResult> ExportToFile(DataSchema Schema, string outputfile)
    {
        var entityImports = new Dictionary<string, EntityImport>();
        foreach (var entitySchema in Schema.Entity)
        {
            _logger.LogInformation("Exporting entity {entityName}", entitySchema.Displayname);

            var exportfields = entitySchema.Fields.Field.Select(f => f.Name).ToList();
            var metadata = await _metadataService.GetEntity(entitySchema.Name);
            var query = new QueryExpression(entitySchema.Name)
            {
                ColumnSet = new ColumnSet(exportfields.ToArray()),

            };
            var entityCollection = await _organizationServiceAsync2.RetrieveAll(query, page: 5000, _logger);

            var data = entityCollection.Entities.Select(e => _recordMapper.Map((metadata, entitySchema, e))).ToList();

            //Add Relationships export

            entityImports[entitySchema.Name] = new EntityImport
            {
                Name = entitySchema.Name,
                Displayname = entitySchema.Displayname,
                Records = new Records { Record = data }
            };

        }
        // Write To File

        return TaskResult.Completed;
    }
}

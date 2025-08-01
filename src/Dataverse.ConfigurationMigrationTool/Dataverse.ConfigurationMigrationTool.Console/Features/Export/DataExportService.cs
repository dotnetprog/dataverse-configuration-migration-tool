using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Extensions.Logging;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export;
public interface IDataExportService
{
    Task<IEnumerable<EntityImport>> ExportEntitiesFromSchema(DataSchema Schema);
}
public class DataExportService : IDataExportService
{
    private readonly ILogger<DataExportService> _logger;
    private readonly IMetadataService _metadataService;
    private readonly IDomainService _domainService;

    public DataExportService(ILogger<DataExportService> logger,
        IMetadataService metadataService,
        IDomainService domainService)
    {
        _logger = logger;
        _metadataService = metadataService;
        _domainService = domainService;
    }

    public async Task<IEnumerable<EntityImport>> ExportEntitiesFromSchema(DataSchema Schema)
    {
        var entityImports = new Dictionary<string, EntityImport>();
        foreach (var entitySchema in Schema.Entity)
        {
            _logger.LogInformation("Exporting entity {entityName}", entitySchema.Displayname);
            var metadata = await _metadataService.GetEntity(entitySchema.Name);
            var data = await _domainService.GetRecords(metadata, entitySchema);
            //Add Relationships export
            var entityRelationShips = new List<M2mrelationship>();
            foreach (var relationship in entitySchema.Relationships.Relationship)
            {
                if (!relationship.ManyToMany)
                {
                    continue;
                }
                var relMD = metadata.ManyToManyRelationships.FirstOrDefault(r => r.IntersectEntityName == relationship.RelatedEntityName);
                var relationships = await _domainService.GetM2mRelationships(relMD);
                entityRelationShips.AddRange(relationships);

            }
            entityImports[entitySchema.Name] = new EntityImport
            {
                Name = entitySchema.Name,
                Displayname = entitySchema.Displayname,
                Records = new Records { Record = data.ToList() },
                M2mrelationships = new M2mrelationships
                {
                    M2mrelationship = entityRelationShips
                }
            };

        }
        // Write To File
        return entityImports.Select(kv => kv.Value).ToList();
    }
}

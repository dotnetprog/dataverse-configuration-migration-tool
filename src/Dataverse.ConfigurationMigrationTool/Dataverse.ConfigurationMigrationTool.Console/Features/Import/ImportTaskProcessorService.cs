using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import;

public enum TaskResult
{
    Completed,
    Failed,
    Requeue
}
public interface IImportTaskProcessorService
{
    Task<TaskResult> Execute(ImportDataTask task, Entities dataImport);
}
public class ImportTaskProcessorService : IImportTaskProcessorService
{
    private readonly IMetadataService metadataService;
    private readonly ILogger<ImportTaskProcessorService> logger;
    private readonly IDataverseValueConverter _dataverseValueConverter;
    private readonly IBulkOrganizationService bulkOrganizationService;
    const int MAX_RETRIES = 3;
    public ImportTaskProcessorService(IMetadataService metadataService,
        ILogger<ImportTaskProcessorService> logger,
        IDataverseValueConverter dataverseValueConverter,
        IBulkOrganizationService bulkOrganizationService)
    {
        this.metadataService = metadataService;
        this.logger = logger;
        _dataverseValueConverter = dataverseValueConverter;
        this.bulkOrganizationService = bulkOrganizationService;
    }

    public async Task<TaskResult> Execute(ImportDataTask task, Entities dataImport)
    {
        var entityToImport = dataImport.Entity.FirstOrDefault(e => e.Name == task.SouceEntityName || e.Name == task.EntitySchema?.Name);
        if (entityToImport == null)
        {
            return TaskResult.Completed;
        }
        var entityMD = await metadataService.GetEntity(entityToImport.Name);
        var result = task.RelationshipSchema != null ?
            await ImportRelationships(entityMD, task, entityToImport) :
            await ImportRecords(entityMD, task, entityToImport);
        return result;
    }

    private async Task<TaskResult> ImportRelationships(EntityMetadata entity, ImportDataTask task, EntityImport entityImport)
    {
        this.logger.LogInformation("Importing relationship : {relationshipname} | for entity source : {entityname}", task.RelationshipSchema.Name, entityImport.Name);
        var relMD = entity.ManyToManyRelationships.FirstOrDefault(m => m.IntersectEntityName == task.RelationshipSchema.Name);
        if (relMD == null) return TaskResult.Failed;

        var rows = entityImport.M2mrelationships.M2mrelationship.Where(m2m => m2m.M2mrelationshipname == task.RelationshipSchema.Name).ToList();

        var requests = rows.SelectMany(r => r.Targetids.Targetid.Select(t =>
        new AssociateRequest
        {
            Target = new EntityReference
            {
                Id = r.Sourceid,
                LogicalName = entityImport.Name
            },
            Relationship = new Relationship(relMD.SchemaName),
            RelatedEntities = new EntityReferenceCollection() {
                new EntityReference
                    {
                        Id = t,
                        LogicalName = r.Targetentityname
                    }
            }

        }));
        var responses = await bulkOrganizationService.ExecuteBulk(requests, ["Cannot insert duplicate key"]);
        var resultTask = responses.Any() ? TaskResult.Failed : TaskResult.Completed;

        foreach (var response in responses)
        {

            var targetRequest = (response.OriginalRequest as AssociateRequest).Target;


            logger.LogError("{logicalname}({id}) Adding relationship ({relationship}) failed because: {fault}", targetRequest.LogicalName, targetRequest.Id, task.RelationshipSchema.Name, response.Fault.Message);


        }


        return resultTask;
    }
    private async Task<TaskResult> ImportRecords(EntityMetadata entity, ImportDataTask task, EntityImport entityImport)
    {
        logger.LogInformation("Importing {entityname} records ({count})", entityImport.Name, entityImport.Records.Record.Count);


        var responses = await ProcessDependantRecords(entityImport.Records.Record, entity, entityImport);
        foreach (var response in responses)
        {

            var targetRequest = (response.OriginalRequest as UpsertRequest).Target;


            logger.LogError("{logicalname}({id}) upsert failed because: {fault}", targetRequest.LogicalName, targetRequest.Id, response.Fault.Message);


        }
        var resultTask = responses.Any() ? TaskResult.Failed : TaskResult.Completed;
        logger.LogInformation("Import Task of {entityname} records terminated in a {State} state", entityImport.Name, resultTask);
        return resultTask;

    }
    private async Task<IEnumerable<OrganizationResponseFaultedResult>> ProcessDependantRecords(IEnumerable<Record> records, EntityMetadata entity, EntityImport entityImport)
    {

        var results = new List<OrganizationResponseFaultedResult>();
        var recordsCanBeProcessed = records.Where(r => !r.Field.Any(f => f.Lookupentity == entityImport.Name &&
                        records.Any(r2 => r2.Id != r.Id && r2.Id.ToString() == f.Value))).Select(r => BuildUpsertRequest(entity, entityImport, r)).ToList();
        logger.LogInformation("Processing {count} records", recordsCanBeProcessed.Count);
        if (recordsCanBeProcessed.Count == 0)
        {
            if (records.Any())
            {
                logger.LogWarning("{count} records skipped because of circular dependancies.", records.Count());
            }
            return results;
        }
        var responses = await bulkOrganizationService.UpsertBulk(recordsCanBeProcessed);
        results.AddRange(responses);

        responses = await ProcessDependantRecords(records.Where(r => !recordsCanBeProcessed.Any(r2 => r.Id == r2.Target.Id)), entity, entityImport);
        results.AddRange(responses);
        return results;
    }
    private UpsertRequest BuildUpsertRequest(EntityMetadata entityMD, EntityImport entityImport, Record record)
    {
        var entity = new Entity(entityImport.Name, record.Id);
        entity[entityMD.PrimaryIdAttribute] = record.Id;
        foreach (var field in record.Field)
        {
            var attrMD = entityMD.Attributes.FirstOrDefault(a => a.LogicalName == field.Name);
            if ((attrMD.IsValidForCreate != true && attrMD.LogicalName != "statecode")
                || attrMD.IsValidForUpdate != true ||
                attrMD.AttributeType == AttributeTypeCode.Owner) continue;
            entity[field.Name] = _dataverseValueConverter.Convert(attrMD, field);
        }
        return new UpsertRequest
        {

            Target = entity
        };
    }

}

using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import
{
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

        public ImportTaskProcessorService(IMetadataService metadataService, ILogger<ImportTaskProcessorService> logger, IDataverseValueConverter dataverseValueConverter, IBulkOrganizationService bulkOrganizationService)
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
            if (task.RelationshipSchema != null)
            {
                //Import relationships

            }
            else
            {
                var entityMD = await metadataService.GetEntity(entityToImport.Name);
                //Import Entities
                return await ImportRecords(entityMD, task, entityToImport);
            }

            return TaskResult.Completed;

        }

        private async Task<TaskResult> ImportRelationships(ImportDataTask task, EntityImport entityImport)
        {
            return TaskResult.Completed;
        }
        private async Task<TaskResult> ImportRecords(EntityMetadata entity, ImportDataTask task, EntityImport entityImport)
        {

            var recordsWithNoSelfDependancies = entityImport.Records.Record.Where(r =>
                                                                        !entityImport.Records.Record.Any(r2 => r2.Id != r.Id &&
                                                                            r2.Field.Any(f => f.Lookupentity != null && f.Value == r.Id.ToString())));
            var recordsWithSelfDependancies = entityImport.Records.Record.Where(r =>
                                                                        entityImport.Records.Record.Any(r2 => r2.Id != r.Id &&
                                                                            r2.Field.Any(f => f.Lookupentity != null && f.Value == r.Id.ToString())));
            var requests = recordsWithNoSelfDependancies.Select(r => BuildUpsertRequest(entity, entityImport, r)).ToList();

            //See if upsert request keep ids

            //implement parallelism and batching
            var responses = await bulkOrganizationService.ExecuteBulk(requests);

            foreach (var response in responses)
            {

                var targetRequest = (response.OriginalRequest as UpsertRequest).Target;


                logger.LogError("{logicalname}({id}) upsert failed because: {fault}", targetRequest.LogicalName, targetRequest.Id, response.Fault.Message);


            }
            var resultTask = responses.Any() ? TaskResult.Failed : TaskResult.Completed;
            logger.LogInformation("Import Task of {entityname} records terminated in a {State} state", entityImport.Name, resultTask);
            return resultTask;

        }

        private UpsertRequest BuildUpsertRequest(EntityMetadata entityMD, EntityImport entityImport, Record record)
        {
            var entity = new Entity(entityImport.Name, record.Id);
            entity[entityMD.PrimaryIdAttribute] = record.Id;
            foreach (var field in record.Field)
            {
                var attrMD = entityMD.Attributes.FirstOrDefault(a => a.LogicalName == field.Name);
                entity[field.Name] = _dataverseValueConverter.Convert(attrMD, field);
            }
            return new UpsertRequest
            {

                Target = entity
            };
        }

    }
}

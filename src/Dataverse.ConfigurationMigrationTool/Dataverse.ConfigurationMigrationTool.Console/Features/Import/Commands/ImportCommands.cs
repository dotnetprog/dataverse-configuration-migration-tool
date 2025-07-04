﻿using Cocona;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.Logging;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;

public class ImportCommands
{
    private readonly ILogger<ImportCommands> _logger;
    private readonly IImportDataProvider _importDataProvider;
    private readonly IValidator<ImportSchema> _schemaValidator;
    private readonly IImportTaskProcessorService _importDataService;

    public ImportCommands(ILogger<ImportCommands> logger,
        IImportDataProvider importDataProvider,
        IValidator<ImportSchema> schemaValidator,
        IImportTaskProcessorService importDataService)
    {
        _logger = logger;
        _importDataProvider = importDataProvider;
        _schemaValidator = schemaValidator;
        _importDataService = importDataService;
    }

    [Command("import")]
    public async Task Import([Option("schema")] string schemafilepath, [Option("data")] string datafilepath)
    {

        var ImportQueue = new Queue<ImportDataTask>();

        var schema = await _importDataProvider.ReadSchemaFromFile(schemafilepath);
        var importdata = await _importDataProvider.ReadFromFile(datafilepath);

        var schemaValidationResult = await _schemaValidator.Validate(schema);
        if (schemaValidationResult.IsError)
        {
            _logger.LogError("Schema failed validation process with {count} failure(s).", schemaValidationResult.Failures.Count);
            throw new Exception("Provided Schema was not valid.");
        }
        _logger.LogInformation("Schema validation succeeded.");

        foreach (var schemaEntity in schema.Entity)
        {
            ImportQueue.Enqueue(new ImportDataTask { EntitySchema = schemaEntity });
            if (schemaEntity.Relationships.Relationship.Any())
            {
                foreach (var relationshipSchema in schemaEntity.Relationships.Relationship)
                {
                    ImportQueue.Enqueue(new ImportDataTask { RelationshipSchema = relationshipSchema, SouceEntityName = schemaEntity.Name });
                }
            }
        }


        while (ImportQueue.Count > 0)
        {
            var importTask = ImportQueue.Dequeue();
            // Requeue la tâche d'import si elle a des dépendances sur d'autres tâches.
            #region Requeuing rules
            if (importTask.EntitySchema != null &&
                ImportQueue.Any(t => t.EntitySchema != null &&
                                     importTask.EntitySchema.Fields.Field.Any(f => f.LookupType != null &&
                                                                            f.LookupType == t.EntitySchema.Name) &&
                                     !t.EntitySchema.Fields.Field.Any(f => f.LookupType != null && f.LookupType == importTask.EntitySchema.Name)))
            {
                ImportQueue.Enqueue(importTask);
                continue;
            }
            if (importTask.RelationshipSchema != null &&
                ImportQueue.Any(t => t.EntitySchema != null &&
                                     t.EntitySchema.Name == importTask.RelationshipSchema.M2mTargetEntity || t.EntitySchema.Name == importTask.SouceEntityName))
            {
                ImportQueue.Enqueue(importTask);
                continue;
            }
            #endregion

            await _importDataService.Execute(importTask, importdata);



        }



    }

}

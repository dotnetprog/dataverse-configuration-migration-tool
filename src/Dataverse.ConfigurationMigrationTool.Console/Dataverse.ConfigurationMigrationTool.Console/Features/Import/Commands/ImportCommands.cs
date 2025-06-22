using Cocona;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.Logging;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;

public class ImportCommands
{
    private readonly ILogger<ImportCommands> _logger;
    private readonly IImportDataProvider _importDataProvider;
    private readonly IValidator<ImportSchema> _schemaValidator;

    public ImportCommands(ILogger<ImportCommands> logger,
        IImportDataProvider importDataProvider,
        IValidator<ImportSchema> schemaValidator)
    {
        _logger = logger;
        _importDataProvider = importDataProvider;
        _schemaValidator = schemaValidator;
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

    }

}

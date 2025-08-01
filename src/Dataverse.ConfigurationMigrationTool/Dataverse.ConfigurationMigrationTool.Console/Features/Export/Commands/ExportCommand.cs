using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Export.Commands;
public class ExportCommand : ICommand
{
    private readonly ILogger<ExportCommand> _logger;
    private readonly ExportCommandOption _options;
    private readonly IValidator<DataSchema> _schemaValidator;
    private readonly IFileDataReader _fileDataReader;
    private readonly IDataExportService _dataExportService;

    public ExportCommand(ILogger<ExportCommand> logger,
        IOptions<ExportCommandOption> options,
        IValidator<DataSchema> schemaValidator,
        IFileDataReader fileDataReader,
        IDataExportService dataExportService)
    {
        _logger = logger;
        _options = options.Value;
        _schemaValidator = schemaValidator;
        _fileDataReader = fileDataReader;
        _dataExportService = dataExportService;
    }

    public async Task Execute() => await Export(_options.schema, _options.output);

    private async Task Export(string schemafilepath, string outputfilepath)
    {

        _logger.LogInformation("Parsing schema file from arguments");
        var schema = await _fileDataReader.ReadAsync<DataSchema>(schemafilepath);

        var schemaValidationResult = await _schemaValidator.Validate(schema);
        if (schemaValidationResult.IsError)
        {
            _logger.LogError("Schema failed validation process with {count} failure(s).", schemaValidationResult.Failures.Count);
            foreach (var failure in schemaValidationResult.Failures)
            {
                _logger.LogError("schema validation failure: {property} => {failure}", failure.PropertyBound, failure.Message);
            }
            throw new Exception("Provided Schema was not valid.");
        }
        _logger.LogInformation("Schema validation succeeded.");

        var entities = await _dataExportService.ExportEntitiesFromSchema(schema);

    }

}

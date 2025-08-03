using Dataverse.ConfigurationMigrationTool.Console.Features.Export;
using Dataverse.ConfigurationMigrationTool.Console.Features.Export.Commands;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Export.Commands;
public class ExportCommandTests
{
    private readonly ILogger<ExportCommand> _logger;
    private readonly ExportCommandOption _options = new ExportCommandOption();
    private readonly IOptions<ExportCommandOption> _optionsWrapper;
    private readonly IValidator<DataSchema> _schemaValidator;
    private readonly IFileDataService _fileDataService;
    private readonly IDataExportService _dataExportService;
    private readonly ExportCommand _exportCommand;
    public ExportCommandTests()
    {
        _logger = Substitute.For<ILogger<ExportCommand>>();
        _optionsWrapper = Substitute.For<IOptions<ExportCommandOption>>();
        _optionsWrapper.Value.Returns(_options);
        _schemaValidator = Substitute.For<IValidator<DataSchema>>();
        _fileDataService = Substitute.For<IFileDataService>();
        _dataExportService = Substitute.For<IDataExportService>(); ;
        _exportCommand = new ExportCommand(_logger, _optionsWrapper, _schemaValidator, _fileDataService, _dataExportService);
    }

    [Fact]
    public async Task GivenACommand_WhenItExecutesWithASchema_ThenItShouldExportData()
    {
        // Arrange
        _options.schema = "schema.json";
        _options.output = "output.json";

        var schema = new DataSchema();
        _fileDataService.ReadAsync<DataSchema>(_options.schema).Returns(Task.FromResult(schema));
        var validationResult = new ValidationResult();
        _schemaValidator.Validate(schema).Returns(Task.FromResult(validationResult));
        var entities = new List<EntityImport>
        {
            new EntityImport { Name = "TestEntity", Displayname = "Test Entity" }
        };
        _dataExportService.ExportEntitiesFromSchema(schema).Returns(Task.FromResult(entities.AsEnumerable()));
        // Act
        await _exportCommand.Execute();
        // Assert
        await _fileDataService.Received(1).WriteAsync(Arg.Is<Entities>(e => e.Entity.Count == 1), _options.output);
    }
    [Fact]
    public async Task GivenACommand_WhenItExecutesWithAnInvalidSchema_ThenItShouldThrowError()
    {
        // Arrange
        _options.schema = "schema.json";
        _options.output = "output.json";

        var schema = new DataSchema();
        _fileDataService.ReadAsync<DataSchema>(_options.schema).Returns(Task.FromResult(schema));
        var validationResult = new ValidationResult()
        {
            Failures = [new("Test", "property failure")]
        };
        _schemaValidator.Validate(schema).Returns(Task.FromResult(validationResult));
        var entities = new List<EntityImport>
        {
            new EntityImport { Name = "TestEntity", Displayname = "Test Entity" }
        };
        _dataExportService.ExportEntitiesFromSchema(schema).Returns(Task.FromResult(entities.AsEnumerable()));
        // Act
        var ex = await _exportCommand.Execute().ShouldThrowAsync<Exception>();
        // Assert
        ex.Message.ShouldBe("Provided Schema was not valid.");
    }

}

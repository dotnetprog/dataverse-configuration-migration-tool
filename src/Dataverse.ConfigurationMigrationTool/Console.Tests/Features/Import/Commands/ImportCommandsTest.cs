using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Commands;
public class ImportCommandsTest
{
    private readonly ILogger<ImportCommands> _logger;
    private readonly IImportDataProvider _importDataProvider;
    private readonly IValidator<DataSchema> _schemaValidator;
    private readonly IImportTaskProcessorService _importDataService;
    private readonly ImportCommands _importCommands;
    const string DataFilePath = "data.json";
    const string SchemaFilePath = "schema.json";
    private ImportCommandOptions _options => new()
    {
        data = DataFilePath,
        schema = SchemaFilePath
    };

    public ImportCommandsTest()
    {
        _logger = Substitute.For<ILogger<ImportCommands>>();
        _importDataProvider = Substitute.For<IImportDataProvider>();
        _schemaValidator = Substitute.For<IValidator<DataSchema>>();
        _importDataService = Substitute.For<IImportTaskProcessorService>();
        var options = Substitute.For<IOptions<ImportCommandOptions>>();
        options.Value.Returns(_options);
        _importCommands = new ImportCommands(_logger,
            _importDataProvider,
            _schemaValidator,
            _importDataService,
            options);
    }

    [Fact]
    public async Task GivenDataToImportWithSchema_WhenTheCommandExecutes_ThenItShouldProcessImportsAccordingly()
    {
        //Arrange
        var importSchema = new DataSchema
        {
            Entity = new()
            {
                FakeSchemas.Account,
                FakeSchemas.Contact,
                FakeSchemas.Opportunity

            }
        };
        var datasets = new Entities
        {
            Entity = new()
            {
                FakeDatasets.AccountSets,
                FakeDatasets.ContactSets,
                FakeDatasets.OpportunitiesSet
            }
        };
        _importDataService.Execute(Arg.Any<ImportDataTask>(), Arg.Any<Entities>())
            .Returns(TaskResult.Completed);
        _importDataProvider.ReadFromFile(DataFilePath).Returns(datasets);
        _importDataProvider.ReadSchemaFromFile(SchemaFilePath).Returns(importSchema);
        _schemaValidator.Validate(importSchema).Returns(new ValidationResult());
        //Act
        await _importCommands.Execute();

        //Assert
        Received.InOrder(async () =>
        {
            await _importDataService.Execute(Arg.Is<ImportDataTask>(x => x.EntitySchema == FakeSchemas.Contact), datasets);
            await _importDataService.Execute(Arg.Is<ImportDataTask>(x => x.EntitySchema == FakeSchemas.Opportunity), datasets);
            await _importDataService.Execute(Arg.Is<ImportDataTask>(x => x.EntitySchema == FakeSchemas.Account), datasets);
            await _importDataService.Execute(Arg.Is<ImportDataTask>(x => x.RelationshipSchema == FakeSchemas.Contact.Relationships.Relationship.First()), datasets);
        });

    }
    [Fact]
    public async Task GivenDataToImportWithSchema_WhenTheCommandExecutesAndFails_ThenItShouldThrowAnError()
    {
        //Arrange
        var importSchema = new DataSchema
        {
            Entity = new()
            {
                FakeSchemas.Account,
                FakeSchemas.Contact,
                FakeSchemas.Opportunity

            }
        };
        var datasets = new Entities
        {
            Entity = new()
            {
                FakeDatasets.AccountSets,
                FakeDatasets.ContactSets,
                FakeDatasets.OpportunitiesSet
            }
        };
        _importDataService.Execute(Arg.Any<ImportDataTask>(), Arg.Any<Entities>())
            .Returns(TaskResult.Failed);
        _importDataProvider.ReadFromFile(DataFilePath).Returns(datasets);
        _importDataProvider.ReadSchemaFromFile(SchemaFilePath).Returns(importSchema);
        _schemaValidator.Validate(importSchema).Returns(new ValidationResult());
        //Act
        Func<Task> act = () => _importCommands.Execute();

        //Assert
        var ex = await act.ShouldThrowAsync<Exception>();
        ex.Message.ShouldBe("Import process failed.");



    }
    [Fact]
    public async Task GivenAnInvalidSchema_WhenTheCommandExecutes_ThenItShouldFailAndLogIssues()
    {
        //Arrange
        var importSchema = new DataSchema
        {
            Entity = new()
            {
                FakeSchemas.Account,
                FakeSchemas.Contact,
                FakeSchemas.Opportunity

            }
        };
        var datasets = new Entities
        {
            Entity = new()
            {
                FakeDatasets.AccountSets,
                FakeDatasets.ContactSets,
                FakeDatasets.OpportunitiesSet
            }
        };
        _importDataService.Execute(Arg.Any<ImportDataTask>(), Arg.Any<Entities>())
            .Returns(TaskResult.Completed);
        _importDataProvider.ReadFromFile(DataFilePath).Returns(datasets);
        _importDataProvider.ReadSchemaFromFile(SchemaFilePath).Returns(importSchema);
        _schemaValidator.Validate(importSchema).Returns(new ValidationResult()
        {
            Failures = new List<ValidationFailure>
            {
                new ("Entity", "Entity is not valid")
            }
        });
        //Act
        Func<Task> act = () => _importCommands.Execute();

        //Assert
        var ex = await act.ShouldThrowAsync<Exception>();
        ex.Message.ShouldBe("Provided Schema was not valid.");
        _logger.ShouldHaveLogged(LogLevel.Error, "Schema failed validation process with 1 failure(s).");


    }
}

using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Commands;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Commands;
public class ImportCommandsTest
{
    private readonly ILogger<ImportCommands> _logger;
    private readonly IImportDataProvider _importDataProvider;
    private readonly IValidator<ImportSchema> _schemaValidator;
    private readonly IImportTaskProcessorService _importDataService;
    private readonly ImportCommands _importCommands;
    const string DataFilePath = "data.json";
    const string SchemaFilePath = "schema.json";

    public ImportCommandsTest()
    {
        _logger = Substitute.For<ILogger<ImportCommands>>();
        _importDataProvider = Substitute.For<IImportDataProvider>();
        _schemaValidator = Substitute.For<IValidator<ImportSchema>>();
        _importDataService = Substitute.For<IImportTaskProcessorService>();
        _importCommands = new ImportCommands(_logger,
            _importDataProvider,
            _schemaValidator,
            _importDataService);
    }

    [Fact]
    public async Task GivenDataToImportWithSchema_WhenTheCommandExecutes_ThenItShouldProcessImportsAccordingly()
    {
        //Arrange
        var importSchema = new ImportSchema
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
        await _importCommands.Import(SchemaFilePath, DataFilePath);

        //Assert
        Received.InOrder(async () =>
        {
            await _importDataService.Execute(Arg.Is<ImportDataTask>(x => x.EntitySchema == FakeSchemas.Contact), datasets);
            await _importDataService.Execute(Arg.Is<ImportDataTask>(x => x.EntitySchema == FakeSchemas.Opportunity), datasets);
            await _importDataService.Execute(Arg.Is<ImportDataTask>(x => x.EntitySchema == FakeSchemas.Account), datasets);
            await _importDataService.Execute(Arg.Is<ImportDataTask>(x => x.RelationshipSchema == FakeSchemas.Contact.Relationships.Relationship.First()), datasets);
        });

    }

}

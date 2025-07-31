using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Services.Filesystem;
public class FileReaderDataImportProviderTests
{
    private readonly IFileDataReader _dataReader;
    private readonly FileReaderDataImportProvider _fileReaderDataImportProvider;
    public FileReaderDataImportProviderTests()
    {
        _dataReader = Substitute.For<IFileDataReader>();
        _fileReaderDataImportProvider = new FileReaderDataImportProvider(_dataReader,
            Substitute.For<ILogger<FileReaderDataImportProvider>>());

    }
    [Fact]
    public async Task GivenAnImportSchema_WhenProviderReadsTheImportSchema_ThenItShouldUseDataReader()
    {
        // Arrange
        var filePath = "test-schema.json";
        var importSchema = new DataSchema();
        _dataReader.ReadAsync<DataSchema>(filePath).Returns(importSchema);
        // Act
        var result = await _fileReaderDataImportProvider.ReadSchemaFromFile(filePath);
        // Assert
        result.ShouldBe(importSchema);
        await _dataReader.Received(1).ReadAsync<DataSchema>(filePath);
    }
    [Fact]
    public async Task GivenAnEntityImport_WhenProviderReadsTheEntityData_ThenItShouldUseDataReader()
    {
        // Arrange
        var filePath = "test-entities.json";
        var entities = new Entities();
        _dataReader.ReadAsync<Entities>(filePath).Returns(entities);
        // Act
        var result = await _fileReaderDataImportProvider.ReadFromFile(filePath);
        // Assert
        result.ShouldBe(entities);
        await _dataReader.Received(1).ReadAsync<Entities>(filePath);
    }


}

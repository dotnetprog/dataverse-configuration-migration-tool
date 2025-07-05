using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Services.Filesystem;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Services.Filesystem;
public class XmlFileDataReaderTests
{
    private readonly XmlFileDataReader _xmlFileDataReader = new XmlFileDataReader();
    [Fact]
    public async Task GivenAnXmlFile_WhenTheDataReaderReadsIt_ThenItShouldReturnDataDeserialized()
    {
        // Arrange
        var filePath = "assets/schema.xml"; // Path to your test XML file
        // Act
        var result = await _xmlFileDataReader.ReadAsync<ImportSchema>(filePath);
        // Assert
        result.ShouldNotBeNull();
        var entity = result.Entity.First();
        entity.Name.ShouldBe("fdn_insuranceproductnature");
    }


}

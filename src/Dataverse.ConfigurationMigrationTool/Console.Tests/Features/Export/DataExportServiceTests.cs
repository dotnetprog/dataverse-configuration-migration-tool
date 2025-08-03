using Dataverse.ConfigurationMigrationTool.Console.Features.Export;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Record = Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain.Record;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Export;
public class DataExportServiceTests
{
    private readonly ILogger<DataExportService> _logger;
    private readonly IMetadataService _metadataService;
    private readonly IDomainService _domainService;
    private readonly DataExportService _dataExportService;

    public DataExportServiceTests()
    {
        _logger = Substitute.For<ILogger<DataExportService>>();
        _metadataService = Substitute.For<IMetadataService>();
        _domainService = Substitute.For<IDomainService>();
        _dataExportService = new DataExportService(_logger, _metadataService, _domainService);
    }
    [Fact]
    public async Task GivenADataExportService_WhenItExportsDataFromSchema_ThenItShouldUseDomainServiceProperly()
    {
        //Arrange

        var schema = FakeSchemas.Contact;
        var metadata = FakeMetadata.Contact;
        _metadataService.GetEntity(metadata.LogicalName).Returns(metadata);

        var records = new List<Record>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Field = [new(){
                    Name = "firstname",
                    Value = "John"
                },
                new(){
                    Name = "lastname",
                    Value = "Doe"
                }],
            },
             new()
            {
                  Id = Guid.NewGuid(),
                Field = [new(){
                    Name = "firstname",
                    Value = "Jane"
                },
                new(){
                    Name = "lastname",
                    Value = "Dane"
                }],
            }
        };
        var rels = new List<M2mrelationship>
        {
            new()
            {
                M2mrelationshipname = "contact_opportunities",
                Sourceid = Guid.NewGuid(),
                Targetentityname = "opportunity",
                Targetentitynameidfield = "opportunityid",
                Targetids = new Targetids
                {
                    Targetid = [Guid.NewGuid(), Guid.NewGuid()]
                }
            }
        };

        _domainService.GetRecords(schema).Returns(records);
        _domainService.GetM2mRelationships(metadata.ManyToManyRelationships.First())
            .Returns(rels);

        //Act
        var result = await _dataExportService.ExportEntitiesFromSchema(new DataSchema
        {
            Entity = [schema]
        });
        //Assert
        var entityImport = result.Single();
        entityImport.Name.ShouldBe(schema.Name);
        entityImport.Displayname.ShouldBe(schema.Displayname);
        entityImport.Records.Record.ShouldBe(records);
        entityImport.M2mrelationships.M2mrelationship.ShouldBe(rels, true);
    }

}

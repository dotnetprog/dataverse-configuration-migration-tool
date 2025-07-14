using Dataverse.ConfigurationMigrationTool.Console.Features.Import;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Import.ValueConverters;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Dataverse.ConfigurationMigrationTool.Console.Tests.FakeBuilders;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import;
public class ImportTaskProcessorServiceTests
{
    private readonly IMetadataService metadataService;
    private readonly ILogger<ImportTaskProcessorService> logger;
    private readonly IDataverseValueConverter _dataverseValueConverter;
    private readonly IBulkOrganizationService bulkOrganizationService;
    private readonly ImportTaskProcessorService importService;

    public ImportTaskProcessorServiceTests()
    {
        this.metadataService = Substitute.For<IMetadataService>();
        this.logger = Substitute.For<ILogger<ImportTaskProcessorService>>();
        _dataverseValueConverter = Substitute.For<IDataverseValueConverter>();
        this.bulkOrganizationService = Substitute.For<IBulkOrganizationService>();
        this.importService = new ImportTaskProcessorService(metadataService, logger, _dataverseValueConverter, bulkOrganizationService);
    }
    [Fact]
    public async Task GivenAnImportTaskWhereEntitySchemaIsNotFound_WhenExecuted_ThenItShouldReturnCompleted()
    {
        // Arrange
        var task = new ImportDataTask
        {
            SouceEntityName = "nonexistent_entity",
            EntitySchema = FakeSchemas.Contact,
            RelationshipSchema = null
        };
        var dataImport = new Entities
        {
            Entity = new List<EntityImport>
            {
                new EntityImport { Name = "account" }
            }
        };
        // Act
        var result = await importService.Execute(task, dataImport);
        // Assert
        result.ShouldBe(TaskResult.Completed);
    }
    [Fact]
    public async Task GivenARelationshipImportTask_WhenExecuted_ThenItShouldAssociateINDataverseAndReturnCompleted()
    {
        // Arrange
        var task = new ImportDataTask
        {
            SouceEntityName = "contact",
            RelationshipSchema = FakeSchemas.Contact.Relationships.Relationship.FirstOrDefault(),
        };
        var dataImport = new Entities
        {
            Entity = new List<EntityImport>
            {
                FakeDatasets.ContactSets
            }
        };
        IEnumerable<AssociateRequest> requestsSent = null;
        bulkOrganizationService.When(x => x.ExecuteBulk(Arg.Any<IEnumerable<AssociateRequest>>(), Arg.Any<IEnumerable<string>>()))
            .Do(x => requestsSent = x.Arg<IEnumerable<AssociateRequest>>());
        var requests = FakeDatasets.ContactSets.M2mrelationships.M2mrelationship.SelectMany(r => r.Targetids.Targetid.Select(t =>
       new AssociateRequest
       {
           Target = new EntityReference
           {
               Id = r.Sourceid,
               LogicalName = task.SouceEntityName,
           },
           Relationship = new Relationship("contact_opportunities"),
           RelatedEntities = new EntityReferenceCollection() {
                new EntityReference
                    {
                        Id = t,
                        LogicalName = r.Targetentityname
                    }
           }

       }));
        metadataService.GetEntity(task.SouceEntityName).Returns(FakeMetadata.Contact);
        // Act
        var result = await importService.Execute(task, dataImport);
        // Assert
        bulkOrganizationService.Received(1).ExecuteBulk(Arg.Any<IEnumerable<AssociateRequest>>(), Arg.Is<IEnumerable<string>>(s => s.Contains("Cannot insert duplicate key")));
        foreach (var request in requestsSent)
        {

            request.Target.ShouldBeEquivalentTo(new EntityReference
            {
                Id = FakeDatasets.ContactSets.M2mrelationships.M2mrelationship.First().Sourceid,
                LogicalName = task.SouceEntityName
            });
            request.Relationship.SchemaName.ShouldBe("contact_opportunities");
        }
        requests.SelectMany(r => r.RelatedEntities)
            .ShouldBeEquivalentTo(requestsSent.SelectMany(r => r.RelatedEntities));
        result.ShouldBe(TaskResult.Completed);
    }
    [Fact]
    public async Task GivenARelationshipImportTask_WhenExecutedAndAnAssociateRequestHasFailed_ThenItShouldReturnFailed()
    {
        // Arrange
        var task = new ImportDataTask
        {
            SouceEntityName = "contact",
            RelationshipSchema = FakeSchemas.Contact.Relationships.Relationship.FirstOrDefault(),
        };
        var dataImport = new Entities
        {
            Entity = new List<EntityImport>
            {
                FakeDatasets.ContactSets
            }
        };
        IEnumerable<AssociateRequest> requestsSent = null;
        bulkOrganizationService.ExecuteBulk(Arg.Any<IEnumerable<AssociateRequest>>(), Arg.Any<IEnumerable<string>>())
            .Returns([new() {
                Fault = new() {
                    Message = "test error"
                },
                OriginalRequest = new AssociateRequest
                {
                    Target = new EntityReference
                    {
                        Id = Guid.NewGuid(),
                        LogicalName = "contact"
                    }
                }
            }]);
        bulkOrganizationService.When(x => x.ExecuteBulk(Arg.Any<IEnumerable<AssociateRequest>>(), Arg.Any<IEnumerable<string>>()))
            .Do(x => requestsSent = x.Arg<IEnumerable<AssociateRequest>>());

        metadataService.GetEntity(task.SouceEntityName).Returns(FakeMetadata.Contact);
        // Act
        var result = await importService.Execute(task, dataImport);
        // Assert
        bulkOrganizationService.Received(1).ExecuteBulk(Arg.Any<IEnumerable<AssociateRequest>>(), Arg.Is<IEnumerable<string>>(s => s.Contains("Cannot insert duplicate key")));

        result.ShouldBe(TaskResult.Failed);
    }
    [Fact]
    public async Task GivenARelationshipImportTaskWhereRelationshipNotFound_WhenExecuted_ThenItShouldReturnFailed()
    {
        // Arrange
        var task = new ImportDataTask
        {
            SouceEntityName = "contact",
            RelationshipSchema = FakeSchemas.Contact.Relationships.Relationship.FirstOrDefault(),
        };
        var dataImport = new Entities
        {
            Entity = new List<EntityImport>
            {
                FakeDatasets.ContactSets
            }
        };
        var emd = new FakeEntityMetadataBuilder("account", "accountid", "name")
            .AddAttribute<StringAttributeMetadata>("accountnumber")
            .AddAttribute<StringAttributeMetadata>("companyname")
            .AddRelationship("account_opportunities", "opportunity")
            .Build();
        metadataService.GetEntity(task.SouceEntityName).Returns(emd);
        // Act
        var result = await importService.Execute(task, dataImport);
        // Assert
        result.ShouldBe(TaskResult.Failed);
    }
}

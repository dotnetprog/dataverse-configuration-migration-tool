using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Dataverse.ConfigurationMigrationTool.Console.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Services.Dataverse;
public class DataverseDomainServiceTests
{
    private readonly IOrganizationServiceAsync2 _orgService;
    private readonly ILogger<DataverseDomainService> _logger;
    private readonly DataverseDomainService _domainService;
    private readonly IOptions<DataverseDomainServiceOptions> _options = Options.Create(new DataverseDomainServiceOptions()
    {
        AllowEmptyFields = false
    });
    public DataverseDomainServiceTests()
    {
        _orgService = Substitute.For<IOrganizationServiceAsync2>();
        _logger = Substitute.For<ILogger<DataverseDomainService>>();
        _domainService = new DataverseDomainService(_orgService, _options, _logger);
    }
    [Fact]
    public async Task GivenADomainService_WhenItExportsWithEntitySchema_ThenItShouldCallDataverseProperly()
    {
        //Arrange
        var schema = FakeSchemas.Account;
        var primaryContactField = new EntityReference("contact", Guid.NewGuid()) { Name = "John Doe" };
        var firstPage = new EntityCollection
        {
            MoreRecords = true
        };
        firstPage.Entities.Add(new(schema.Name)
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test Account",
            ["primarycontactid"] = primaryContactField
        });
        var secondPage = new EntityCollection()
        {
            MoreRecords = false
        };
        secondPage.Entities.Add(new(schema.Name)
        {
            Id = Guid.NewGuid(),
            ["name"] = "Test2 Account",
            ["primarycontactid"] = primaryContactField
        });
        _orgService.RetrieveMultipleAsync(Arg.Any<QueryExpression>(), Arg.Any<CancellationToken>())
            .Returns(r =>
            {

                var arg = r.Arg<QueryExpression>();
                if (arg.PageInfo.PageNumber == 1)
                {
                    return Task.FromResult(firstPage);
                }
                else
                {
                    return Task.FromResult(secondPage);
                }
            });

        //Act
        var result = (await _domainService.GetRecords(schema)).ToList();

        //Assert
        result.Count.ShouldBe(2);
        result[0].Id.ShouldBe(firstPage.Entities[0].Id);
        result[0].Field.ShouldContain(f => f.Name == "name" && f.Value.ToString() == "Test Account");
        result[0].Field.ShouldContain(f => f.Name == "primarycontactid" &&
                                            f.Value == primaryContactField.Id.ToString()
                                            && f.Lookupentityname == primaryContactField.Name
                                            && f.Lookupentity == primaryContactField.LogicalName);
        result[1].Id.ShouldBe(secondPage.Entities[0].Id);
        result[1].Field.ShouldContain(f => f.Name == "name" && f.Value.ToString() == "Test2 Account");
        result[1].Field.ShouldContain(f => f.Name == "primarycontactid" &&
                                            f.Value == primaryContactField.Id.ToString()
                                            && f.Lookupentityname == primaryContactField.Name
                                            && f.Lookupentity == primaryContactField.LogicalName);
    }
    [Fact]
    public async Task GivenADomainService_WhenItExportsWithEntitySchemaWithNoFields_ThenItShouldNotCallDataverse()
    {
        //Arrange
        var schema = new EntitySchema()
        {
            Name = "account",
        };

        //Act
        var result = await _domainService.GetRecords(schema);

        //Assert
        result.ShouldBeEmpty();
        _logger.ShouldHaveLogged(LogLevel.Warning, $"No fields specified for export in schema for entity {schema.Name}");

    }
    [Fact]
    public async Task GivenADomainService_WhenItExportsM2mRelationships_ThenItShouldCallDataverseProperly()
    {
        //Arrange
        var metadata = FakeMetadata.Contact;
        var m2mMetadata = metadata.ManyToManyRelationships.First();
        var response = new EntityCollection();
        response.Entities.Add(new(m2mMetadata.IntersectEntityName)
        {
            Id = Guid.NewGuid(),
            [m2mMetadata.Entity1IntersectAttribute] = Guid.NewGuid(),
            [m2mMetadata.Entity2IntersectAttribute] = Guid.NewGuid()
        });

        _orgService.RetrieveMultipleAsync(Arg.Any<QueryExpression>(), Arg.Any<CancellationToken>())
            .Returns(response);
        //Act
        var result = (await _domainService.GetM2mRelationships(m2mMetadata)).ToList();
        //Assert
        result.Count.ShouldBe(1);
        var entity = response.Entities[0];
        result[0].Sourceid.ShouldBe(entity.GetAttributeValue<Guid>(m2mMetadata.Entity1IntersectAttribute));
        result[0].Targetentityname.ShouldBe(m2mMetadata.Entity2LogicalName);
        result[0].Targetentitynameidfield.ShouldBe(m2mMetadata.Entity2IntersectAttribute);
        result[0].Targetids.Targetid.Count.ShouldBe(1);
        result[0].Targetids.Targetid[0].ShouldBe(entity.GetAttributeValue<Guid>(m2mMetadata.Entity2IntersectAttribute));

    }
}

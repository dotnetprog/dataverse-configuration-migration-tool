using Dataverse.ConfigurationMigrationTool.Console.Services.Metadata;
using Dataverse.ConfigurationMigrationTool.Console.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using System.ServiceModel;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Services.Dataverse;

public class DataverseMetadataServiceTests
{

    private readonly IOrganizationServiceAsync2 _orgService;
    private readonly DataverseMetadataService metadataService;
    private readonly EntityMetadata AccountMetadata;
    private readonly EntityMetadata IntersectEntityMetadata;
    private readonly ILogger<DataverseMetadataService> _logger;

    public DataverseMetadataServiceTests()
    {
        _orgService = Substitute.For<IOrganizationServiceAsync2>();
        _logger = Substitute.For<ILogger<DataverseMetadataService>>();
        metadataService = new DataverseMetadataService(_orgService, _logger);
        AccountMetadata = new EntityMetadata
        {
            LogicalName = "account",
            SchemaName = "account",
            DisplayName = new Label("Account", 1033),
        };
        IntersectEntityMetadata = new EntityMetadata
        {
            LogicalName = "account_contact",
            SchemaName = "account_contact",
            DisplayName = new Label("Account Contact", 1033)
        };
        typeof(EntityMetadata).GetProperty("ManyToManyRelationships")?.SetValue(IntersectEntityMetadata, new ManyToManyRelationshipMetadata[]
            {
                new ManyToManyRelationshipMetadata
                {
                    SchemaName = "account_contact",
                    Entity1LogicalName = "account",
                    Entity2LogicalName = "contact",
                    IntersectEntityName = "account_contact"
                }
            });
    }
    [Fact]
    public async Task GivenAnEntityInDataverse_WhenServicesGetsItsMetadata_ThenItShouldReturnResultFromDataverse()
    {
        //Arrange

        var EntityResponse = new RetrieveEntityResponse
        {
            ["EntityMetadata"] = AccountMetadata
        };
        _orgService.ExecuteAsync(Arg.Is<RetrieveEntityRequest>(x => x.LogicalName == AccountMetadata.LogicalName))
            .Returns(Task.FromResult((OrganizationResponse)EntityResponse));


        //Act
        var result = await metadataService.GetEntity(AccountMetadata.LogicalName);
        //Assert
        result.ShouldBe(AccountMetadata);
    }
    [Fact]
    public async Task GivenAnEntityThatIsNotInDataverse_WhenServicesGetsItsMetadata_ThenItShouldReturnNullAndLogError()
    {
        //Arrange

        var faultException = new FaultException<OrganizationServiceFault>(
            new OrganizationServiceFault
            {
                Message = "Entity not found",
                ErrorCode = -2147220970
            });
        _orgService.ExecuteAsync(Arg.Any<RetrieveEntityRequest>())
            .ThrowsAsyncForAnyArgs(faultException);


        //Act
        var result = await metadataService.GetEntity(AccountMetadata.LogicalName);
        //Assert
        result.ShouldBeNull();
        _logger.ShouldHaveLoggedException(LogLevel.Trace, faultException);
    }
    [Fact]
    public async Task GivenAnIntersectEntityInDataverse_WhenServicesGetsItsMetadata_ThenItShouldReturnResultFromDataverse()
    {
        //Arrange

        var EntityResponse = new RetrieveEntityResponse
        {
            ["EntityMetadata"] = IntersectEntityMetadata
        };
        _orgService.ExecuteAsync(Arg.Is<RetrieveEntityRequest>(x => x.LogicalName == IntersectEntityMetadata.LogicalName))
            .Returns(Task.FromResult((OrganizationResponse)EntityResponse));


        //Act
        var result = await metadataService.GetRelationShipM2M(IntersectEntityMetadata.LogicalName);
        //Assert
        result.ShouldBe(IntersectEntityMetadata.ManyToManyRelationships.First());
    }



}

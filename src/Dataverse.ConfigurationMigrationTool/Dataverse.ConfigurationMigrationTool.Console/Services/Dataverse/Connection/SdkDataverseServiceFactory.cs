using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Connection;

public class SdkDataverseServiceFactory : IDataverseClientFactory
{
    private readonly SdkDataverseServiceFactoryOptions _options;
    private ILogger<SdkDataverseServiceFactory> _logger;
    /// <summary>
    /// The default Microsoft App ID used for interactive login if no other client ID is provided.
    /// Source: https://github.com/microsoft/PowerApps-Samples/tree/master/dataverse/orgsvc/CSharp-NETCore/ServiceClient
    /// </summary>
    const string DefaultMicrosoftAppId = "51f81489-12ee-4a9e-aaae-a2591f45987d";
    const string DefaultMicrosoftRedirectUri = "http://localhost";

    public SdkDataverseServiceFactory(IOptions<SdkDataverseServiceFactoryOptions> options,
        ILogger<SdkDataverseServiceFactory> logger)
    {
        _options = options.Value;
        _logger = logger;
    }
    public IOrganizationServiceAsync2 Create()
    {
        _logger.LogWarning("Creating a new ServiceClient with Url: {url}", _options.Url);
        if (_options.InteractiveLogin)
        {
            var cstring = $"AuthType=OAuth;Url={_options.Url};RedirectUri={DefaultMicrosoftRedirectUri};AppId={DefaultMicrosoftAppId};LoginPrompt=Auto";
            return new ServiceClient(dataverseConnectionString: cstring,
                logger: _logger);
        }
        var serviceClient = new ServiceClient(
            new Uri(_options.Url),
            _options.ClientId.ToString(),
            _options.ClientSecret, true, _logger);

        if (!serviceClient.IsReady)
        {
            throw new InvalidOperationException("Could not resolve dataverse connection", serviceClient.LastException);
        }
        return serviceClient;
    }
}

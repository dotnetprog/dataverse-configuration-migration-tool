using Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;

public class SdkDataverseServiceFactory : IDataverseClientFactory
{
    private readonly SdkDataverseServiceFactoryOptions _options;
    private ILogger<SdkDataverseServiceFactory> _logger;

    public SdkDataverseServiceFactory(IOptions<SdkDataverseServiceFactoryOptions> options,
        ILogger<SdkDataverseServiceFactory> logger)
    {
        this._options = options.Value;
        _logger = logger;
    }
    public IOrganizationServiceAsync2 Create()
    {
        // throw new InvalidOperationException($"test: {_options.Url} {_options.ClientId} {_options.ClientSecret}");
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

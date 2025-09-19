namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse.Configuration;

public class SdkDataverseServiceFactoryOptions
{
    public Guid ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Url { get; set; }
    public bool InteractiveLogin { get; set; }

}

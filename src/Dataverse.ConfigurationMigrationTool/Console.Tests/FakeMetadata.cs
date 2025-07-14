using Dataverse.ConfigurationMigrationTool.Console.Tests.FakeBuilders;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests;
internal static class FakeMetadata
{
    public static EntityMetadata Contact =>
        new FakeEntityMetadataBuilder("contact", "contactid", "fullname")
            .AddAttribute<StringAttributeMetadata>("firstname")
            .AddAttribute<StringAttributeMetadata>("lastname")
            .AddRelationship("contact_opportunities", "opportunity")
            .Build();
    public static EntityMetadata Account =>
        new FakeEntityMetadataBuilder("account", "accountid", "name")
            .AddAttribute<StringAttributeMetadata>("name")
            .AddAttribute<LookupAttributeMetadata>("parentaccountid", (md) =>
            {
                md.Targets = new[] { "account" };
            })
            .Build();

}

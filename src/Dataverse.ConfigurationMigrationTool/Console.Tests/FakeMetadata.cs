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

}

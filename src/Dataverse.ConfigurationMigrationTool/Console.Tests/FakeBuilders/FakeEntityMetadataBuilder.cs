using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.FakeBuilders;
internal class FakeEntityMetadataBuilder
{
    private string EntityName { get; }
    private string PrimaryIdField { get; }
    private string PrimaryNameField { get; }
    private List<AttributeMetadata> Fields { get; } = new List<AttributeMetadata>();
    private List<ManyToManyRelationshipMetadata> ManyToManyRelationships { get; } = new List<ManyToManyRelationshipMetadata>();

    public FakeEntityMetadataBuilder(string entityName, string primaryIdField, string primaryNameField)
    {
        EntityName = entityName;
        PrimaryIdField = primaryIdField;
        PrimaryNameField = primaryNameField;
    }
    public FakeEntityMetadataBuilder AddAttribute<T>(string logicalName, Action<T> configureMD = null)
        where T : AttributeMetadata, new()
    {
        var attribute = new T
        {
            LogicalName = logicalName
        };
        attribute.IsValidForUpdate = true;
        attribute.IsValidForCreate = true;
        configureMD?.Invoke(attribute);

        Fields.Add(attribute);
        return this;
    }
    public FakeEntityMetadataBuilder AddRelationship(string SchemaName, string TargetEntity)
    {
        var relationship = new ManyToManyRelationshipMetadata
        {
            Entity1LogicalName = EntityName,
            Entity2LogicalName = TargetEntity,
            IntersectEntityName = SchemaName,
            SchemaName = SchemaName
        };
        ManyToManyRelationships.Add(relationship);
        return this;
    }

    public EntityMetadata Build()
    {

        var entityMd = new EntityMetadata()
        {
            LogicalName = EntityName
        };
        typeof(EntityMetadata).GetProperty(nameof(EntityMetadata.PrimaryIdAttribute))!
            .SetValue(entityMd, PrimaryIdField);
        typeof(EntityMetadata).GetProperty(nameof(EntityMetadata.PrimaryNameAttribute))!
            .SetValue(entityMd, PrimaryNameField);
        typeof(EntityMetadata).GetProperty(nameof(EntityMetadata.Attributes))!
            .SetValue(entityMd, Fields.ToArray());
        typeof(EntityMetadata).GetProperty(nameof(EntityMetadata.ManyToManyRelationships))!
            .SetValue(entityMd, ManyToManyRelationships.ToArray());
        return entityMd;
    }
}

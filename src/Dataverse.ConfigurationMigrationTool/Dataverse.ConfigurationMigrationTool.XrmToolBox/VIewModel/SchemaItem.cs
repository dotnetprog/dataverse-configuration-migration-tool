namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.VIewModel
{
    public enum SchemaItemType
    {
        Field,
        Relationship
    }
    public abstract class SchemaItem
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public abstract SchemaItemType schemaItemType { get; }
    }
    public class FieldSchemaItem : SchemaItem
    {

        public FieldComponentMetadataViewModel OriginalItem { get; set; }

        public override SchemaItemType schemaItemType => SchemaItemType.Field;
    }
    public class RelationshipSchemaItem : SchemaItem
    {

        public RelationshipComponentMetadataViewModel OriginalItem { get; set; }

        public override SchemaItemType schemaItemType => SchemaItemType.Relationship;
    }
}

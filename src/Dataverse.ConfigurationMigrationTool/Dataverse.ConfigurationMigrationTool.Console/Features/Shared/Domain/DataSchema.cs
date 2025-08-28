using System.Collections.Generic;
using System.Xml.Serialization;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain
{
    [XmlRoot(ElementName = "field")]
    public class FieldSchema
    {

        [XmlAttribute(AttributeName = "displayname")]
        public string Displayname { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "customfield")]
        public bool Customfield { get; set; }

        [XmlAttribute(AttributeName = "primaryKey")]
        public bool PrimaryKey { get; set; }

        [XmlAttribute(AttributeName = "lookupType")]
        public string LookupType { get; set; }
        public bool ShouldSerializeCustomfield()
        {
            return Customfield;
        }
        public bool ShouldSerializePrimaryKey()
        {
            return PrimaryKey;
        }
    }

    [XmlRoot(ElementName = "fields")]
    public class FieldsSchema
    {

        [XmlElement(ElementName = "field")]
        public List<FieldSchema> Field { get; set; } = new List<FieldSchema>();
    }

    [XmlRoot(ElementName = "relationship")]
    public class RelationshipSchema
    {

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "manyToMany")]
        public bool ManyToMany { get; set; }

        [XmlAttribute(AttributeName = "isreflexive")]
        public bool Isreflexive { get; set; }

        [XmlAttribute(AttributeName = "relatedEntityName")]
        public string RelatedEntityName { get; set; }

        [XmlAttribute(AttributeName = "m2mTargetEntity")]
        public string M2mTargetEntity { get; set; }

        [XmlAttribute(AttributeName = "m2mTargetEntityPrimaryKey")]
        public string M2mTargetEntityPrimaryKey { get; set; }
    }

    [XmlRoot(ElementName = "relationships")]
    public class RelationshipsSchema
    {

        [XmlElement(ElementName = "relationship")]
        public List<RelationshipSchema> Relationship { get; set; } = new List<RelationshipSchema>();
    }

    [XmlRoot(ElementName = "entity")]
    public class EntitySchema
    {

        [XmlElement(ElementName = "fields")]
        public FieldsSchema Fields { get; set; } = new FieldsSchema();

        [XmlElement(ElementName = "relationships")]
        public RelationshipsSchema Relationships { get; set; } = new RelationshipsSchema();

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "displayname")]
        public string Displayname { get; set; }

        [XmlAttribute(AttributeName = "etc")]
        public int Etc { get; set; }

        [XmlAttribute(AttributeName = "primaryidfield")]
        public string Primaryidfield { get; set; }

        [XmlAttribute(AttributeName = "primarynamefield")]
        public string Primarynamefield { get; set; }

        [XmlAttribute(AttributeName = "disableplugins")]
        public bool Disableplugins { get; set; }
    }

    [XmlRoot(ElementName = "entities")]
    public class DataSchema
    {

        [XmlElement(ElementName = "entity")]
        public List<EntitySchema> Entity { get; set; }
    }
}






using System.Xml.Serialization;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;

// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(Entities));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (Entities)serializer.Deserialize(reader);
// }

[XmlRoot(ElementName = "field")]
public class Field
{

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }

    [XmlAttribute(AttributeName = "value")]
    public string Value { get; set; }

    [XmlAttribute(AttributeName = "lookupentity")]
    public string Lookupentity { get; set; }

    [XmlAttribute(AttributeName = "lookupentityname")]
    public string Lookupentityname { get; set; }
}

[XmlRoot(ElementName = "record")]
public class Record
{

    [XmlElement(ElementName = "field")]
    public List<Field> Field { get; set; }

    [XmlAttribute(AttributeName = "id")]
    public Guid Id { get; set; }
}

[XmlRoot(ElementName = "records")]
public class Records
{

    [XmlElement(ElementName = "record")]
    public List<Record> Record { get; set; }
}

[XmlRoot(ElementName = "targetids")]
public class Targetids
{

    [XmlElement(ElementName = "targetid")]
    public List<Guid> Targetid { get; set; }
}

[XmlRoot(ElementName = "m2mrelationship")]
public class M2mrelationship
{

    [XmlElement(ElementName = "targetids")]
    public Targetids Targetids { get; set; }

    [XmlAttribute(AttributeName = "sourceid")]
    public Guid Sourceid { get; set; }

    [XmlAttribute(AttributeName = "targetentityname")]
    public string Targetentityname { get; set; }

    [XmlAttribute(AttributeName = "targetentitynameidfield")]
    public string Targetentitynameidfield { get; set; }

    [XmlAttribute(AttributeName = "m2mrelationshipname")]
    public string M2mrelationshipname { get; set; }

}

[XmlRoot(ElementName = "m2mrelationships")]
public class M2mrelationships
{

    [XmlElement(ElementName = "m2mrelationship")]
    public List<M2mrelationship> M2mrelationship { get; set; }
}

[XmlRoot(ElementName = "entity")]
public class EntityImport
{

    [XmlElement(ElementName = "records")]
    public Records Records { get; set; }

    [XmlElement(ElementName = "m2mrelationships")]
    public M2mrelationships M2mrelationships { get; set; }

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }

    [XmlAttribute(AttributeName = "displayname")]
    public string Displayname { get; set; }

}

[XmlRoot(ElementName = "entities")]
public class Entities
{

    [XmlElement(ElementName = "entity")]
    public List<EntityImport> Entity { get; set; }
}



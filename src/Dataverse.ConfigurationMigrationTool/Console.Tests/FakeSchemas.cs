using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests;
internal static class FakeSchemas
{
    public static readonly EntitySchema Account = new EntitySchema()
    {
        Name = "account",
        Primaryidfield = "accountid",
        Primarynamefield = "name",
        Fields = new FieldsSchema
        {
            Field = new List<FieldSchema>
                   {
                       new FieldSchema
                       {
                           Name = "name",
                           Type = "string"

                       },
                       new FieldSchema
                       {
                           Name = "primarycontactid",
                           Type = "entityreference",
                           Customfield = true,
                           Displayname = "Primary Contact",
                           LookupType = "contact",
                           PrimaryKey = false
                       }
                   }
        },
    };
    public static readonly EntitySchema Opportunity = new EntitySchema()
    {
        Name = "opportunity",
        Primaryidfield = "opportunityid",
        Primarynamefield = "name",
        Displayname = "Opportunity",
        Fields = new FieldsSchema
        {
            Field = new List<FieldSchema>
                   {
                       new FieldSchema
                       {
                           Name = "name",
                           Type = "string"
                       },
                        new FieldSchema
                       {
                           Name = "estimatedvalue",
                           Type = "money"
                       }
                   }
        },
    };
    public static readonly EntitySchema Contact = new EntitySchema()
    {
        Name = "contact",
        Primaryidfield = "contactid",
        Primarynamefield = "fullname",
        Displayname = "Contact",
        Etc = 2,
        Disableplugins = false,
        Relationships = new RelationshipsSchema
        {
            Relationship = new List<RelationshipSchema>
                   {
                       new RelationshipSchema
                       {
                            Name = "contact_opportunities",
                            Isreflexive = false,
                            M2mTargetEntity = "opportunity",
                            M2mTargetEntityPrimaryKey = "opportunityid",
                            ManyToMany = true,
                            RelatedEntityName = "contact_opportunities"
                       }
                   }
        },
        Fields = new FieldsSchema
        {
            Field = new List<FieldSchema>
                   {
                       new FieldSchema
                       {
                           Name = "firstname",
                           Type = "string"

                       },
                        new FieldSchema
                       {
                           Name = "lastname",
                           Type = "string"

                       }
                   }
        },
    };

}

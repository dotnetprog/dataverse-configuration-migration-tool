using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests;
internal static class FakeDatasets
{
    public static readonly Guid[] AccountIds = new[]
    {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
    };
    public static readonly Guid[] ContactIds = new[]
  {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
    };
    public static readonly Guid[] OpportunityIds = new[]
    {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
    };
    public static readonly EntityImport AccountSets = new()
    {
        Displayname = "Account",
        Name = "account",
        Records = new()
        {
            Record = new()
            {
                new(){
                    Id = AccountIds[0],
                    Field = new(){
                        new()
                        {
                            Name = "name",
                            Value = "Account 1"
                        },
                        new()
                        {
                            Name = "primarycontactid",
                            Value = ContactIds[0].ToString(),
                            Lookupentity = "contact",
                            Lookupentityname = "John Doe"
                        }
                    }
                }
            }
        }
    };
    public static readonly EntityImport SelfHiearchyAccountSets = new()
    {
        Displayname = "Account",
        Name = "account",
        Records = new()
        {
            Record = new()
            {
                new(){
                    Id = AccountIds[0],
                    Field = new(){
                        new()
                        {
                            Name = "name",
                            Value = "Account 1"
                        },
                        new()
                        {
                            Name = "parentaccountid",
                            Value = AccountIds[1].ToString(),
                            Lookupentity = "account",
                            Lookupentityname = "Account 2"
                        }
                    }
                },
                new(){
                    Id = AccountIds[1],
                    Field = new(){
                        new()
                        {
                            Name = "name",
                            Value = "Account 2"
                        },
                         new()
                        {
                            Name = "parentaccountid",
                            Value = AccountIds[2].ToString(),
                            Lookupentity = "account",
                            Lookupentityname = "Account 3"
                        }
                    }
                },
                 new(){
                    Id = AccountIds[2],
                    Field = new(){
                        new()
                        {
                            Name = "name",
                            Value = "Account 3"
                        },
                         new()
                        {
                            Name = "parentaccountid",
                            Value = AccountIds[3].ToString(),
                            Lookupentity = "account",
                            Lookupentityname = "Account 4"
                        }
                    }
                },
                  new(){
                    Id = AccountIds[3],
                    Field = new(){
                        new()
                        {
                            Name = "name",
                            Value = "Account 4"
                        }
                    }
                }
            }
        }
    };
    public static readonly EntityImport CIrcularSelfHiearchyAccountSets = new()
    {
        Displayname = "Account",
        Name = "account",
        Records = new()
        {
            Record = new()
            {
                new(){
                    Id = AccountIds[0],
                    Field = new(){
                        new()
                        {
                            Name = "name",
                            Value = "Account 1"
                        },
                        new()
                        {
                            Name = "parentaccountid",
                            Value = AccountIds[1].ToString(),
                            Lookupentity = "account",
                            Lookupentityname = "Account 2"
                        }
                    }
                },
                new(){
                    Id = AccountIds[1],
                    Field = new(){
                        new()
                        {
                            Name = "name",
                            Value = "Account 2"
                        },
                         new()
                        {
                            Name = "parentaccountid",
                            Value = AccountIds[0].ToString(),
                            Lookupentity = "account",
                            Lookupentityname = "Account 1"
                        }
                    }
                }
            }
        }
    };
    public static readonly EntityImport OpportunitiesSet = new()
    {
        Displayname = "Opportunity",
        Name = "opportunity",
        Records = new()
        {
            Record = new()
            {
                new(){
                    Id = OpportunityIds[0],
                    Field = new(){
                        new()
                        {
                            Name = "name",
                            Value = "Opportunity 1"
                        },
                         new()
                        {
                            Name = "estimatedvalue",
                            Value = "4576.23"
                        },
                    }
                }
            }
        }
    };
    public static readonly EntityImport ContactSets = new()
    {
        Displayname = "Contact",
        Name = "contact",
        M2mrelationships = new M2mrelationships
        {
            M2mrelationship = new()
            {
                new()
                {
                   M2mrelationshipname = "contact_opportunities",
                   Sourceid = ContactIds[0],
                   Targetentityname = "opportunity",
                   Targetentitynameidfield = "opportunityid",
                   Targetids = new(){
                       Targetid = new() {
                           OpportunityIds[0],
                       }
                   }
                }
            }
        },
        Records = new()
        {
            Record = new()
            {
                new(){
                    Id = ContactIds[0],
                    Field = new(){
                        new()
                        {
                            Name = "firstname",
                            Value = "John"
                        },
                        new()
                        {
                            Name = "lastname",
                            Value = "Doe"
                        }

                    }
                }
            }
        }
    };
}

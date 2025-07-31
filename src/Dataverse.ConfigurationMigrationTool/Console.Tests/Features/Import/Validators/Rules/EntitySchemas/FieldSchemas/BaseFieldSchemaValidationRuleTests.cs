using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators.Rules.EntitySchemas.FieldSchemas;
using Microsoft.Xrm.Sdk.Metadata;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Import.Validators.Rules.EntitySchemas.FieldSchemas;
public abstract class BaseFieldSchemaValidationRuleTests<TRule> where TRule : IFieldSchemaValidationRule
{
    protected TMetadata CreateAttributeMetadata<TMetadata>() where TMetadata : AttributeMetadata
    {
        var md = Activator.CreateInstance<TMetadata>();
        return md;
    }
    protected Task<RuleResult> ExecuteRule(FieldSchema fieldSchema, AttributeMetadata attributeMetadata)
    {
        var rule = Activator.CreateInstance<TRule>();
        return rule.Validate(fieldSchema, attributeMetadata);
    }

}

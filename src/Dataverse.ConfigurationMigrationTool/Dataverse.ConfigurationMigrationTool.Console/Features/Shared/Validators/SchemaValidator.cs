using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators;

public class SchemaValidator : IValidator<DataSchema>
{
    private readonly IValidator<EntitySchema> entitySchemaValidator;

    public SchemaValidator(IValidator<EntitySchema> entitySchemaValidator)
    {

        this.entitySchemaValidator = entitySchemaValidator;
    }

    public async Task<ValidationResult> Validate(DataSchema value)
    {
        var failures = new List<ValidationFailure>();
        foreach (var entitySchema in value.Entity)
        {

            var validationResult = await entitySchemaValidator.Validate(entitySchema);
            failures.AddRange(validationResult.Failures);

        }

        return new ValidationResult
        {
            Failures = failures
        };


    }
}

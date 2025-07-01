using Dataverse.ConfigurationMigrationTool.Console.Features.Import.Model;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators
{
    public class SchemaValidator : IValidator<ImportSchema>
    {
        private readonly IValidator<EntitySchema> entitySchemaValidator;

        public SchemaValidator(IValidator<EntitySchema> entitySchemaValidator)
        {

            this.entitySchemaValidator = entitySchemaValidator;
        }

        public async Task<ValidationResult> Validate(ImportSchema value)
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
}

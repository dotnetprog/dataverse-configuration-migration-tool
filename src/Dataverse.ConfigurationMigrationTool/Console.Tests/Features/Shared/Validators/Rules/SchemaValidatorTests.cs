using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Domain;
using Dataverse.ConfigurationMigrationTool.Console.Features.Shared.Validators;
using NSubstitute;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Features.Shared.Validators.Rules;
public class SchemaValidatorTests
{
    private readonly IValidator<EntitySchema> entityValidator = Substitute.For<IValidator<EntitySchema>>();
    private readonly IValidator<DataSchema> validator;
    public SchemaValidatorTests()
    {
        validator = new SchemaValidator(entityValidator);
    }

    [Fact]
    public async Task GivenValidSchema_WhenValidated_ThenShouldReturnSuccess()
    {
        // Arrange
        var schema = new DataSchema
        {
            Entity = new List<EntitySchema>
            {
                FakeSchemas.Account,
                FakeSchemas.Opportunity,
                FakeSchemas.Contact
            }
        };
        entityValidator.Validate(Arg.Any<EntitySchema>()).Returns(new ValidationResult());
        // Act
        var result = await validator.Validate(schema);
        // Assert
        result.IsError.ShouldBeFalse();
        result.Failures.ShouldBeEmpty();
    }


}

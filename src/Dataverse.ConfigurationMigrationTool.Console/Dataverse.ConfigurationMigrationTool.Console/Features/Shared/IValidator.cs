namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared
{
    public interface IValidator<T>
    {

        Task<ValidationResult> Validate(T value);
    }

    public class ValidationResult
    {
        public bool IsError => Failures?.Any() ?? false;
        IReadOnlyCollection<ValidationFailure> Failures { get; set; }
    }
    public class ValidationFailure
    {
        public string Message { get; set; }
        public string? PropertyBound { get; set; }
    }
}

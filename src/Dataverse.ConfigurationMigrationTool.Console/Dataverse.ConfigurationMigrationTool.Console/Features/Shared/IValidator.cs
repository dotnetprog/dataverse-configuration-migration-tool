namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared
{
    public interface IValidator<T>
    {

        Task<ValidationResult> Validate(T value);
    }

    public class ValidationResult
    {
        public bool IsError => Failures?.Any() ?? false;
        public IReadOnlyCollection<ValidationFailure> Failures { get; set; } = new List<ValidationFailure>();
    }
    public class ValidationFailure
    {
        public ValidationFailure(string message, string? propertyBound = null)
        {
            Message = message;
            PropertyBound = propertyBound;
        }
        public ValidationFailure() { }
        public string Message { get; set; }
        public string? PropertyBound { get; set; }
    }
}

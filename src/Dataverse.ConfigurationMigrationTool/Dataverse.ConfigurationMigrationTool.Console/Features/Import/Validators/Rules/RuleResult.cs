namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Validators.Rules
{
    public class RuleResult
    {
        private RuleResult() { }
        private RuleResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
        public string ErrorMessage { get; }
        public static RuleResult Success() => new();

        public static RuleResult Failure(string errorMessage) => new(errorMessage);

    }
}

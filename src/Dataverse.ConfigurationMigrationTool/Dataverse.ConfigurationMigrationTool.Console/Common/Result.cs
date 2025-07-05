namespace Dataverse.ConfigurationMigrationTool.Console.Common
{
    public class Result<TValue, TFailure>
    {
        public TValue? Value { get; set; }
        public TFailure? Failure { get; set; }
        public bool IsSuccess => Value != null;
        public bool IsFailure => Failure != null;
        public Result(TValue value)
        {
            this.Value = value;
        }
        public Result(TFailure failure)
        {
            Failure = failure;
        }
    }

}

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
[AttributeUsage(AttributeTargets.Class)]
public class CommandVerbAttribute : Attribute
{
    public string Verb { get; }
    public CommandVerbAttribute(string verb)
    {
        Verb = verb ?? throw new ArgumentNullException(nameof(verb), "Verb cannot be null");
    }
}

using Microsoft.Extensions.Configuration;

namespace Dataverse.ConfigurationMigrationTool.Console.ConfigurationProviders;
public class CustomCommandLineConfigurationProvider : ConfigurationProvider
{
    private readonly Dictionary<string, string>? _switchMappings;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="args">The command line args.</param>
    /// <param name="switchMappings">The switch mappings.</param>
    public CustomCommandLineConfigurationProvider(IEnumerable<string> args, IDictionary<string, string>? switchMappings = null, IEnumerable<string>? flagMappings = null)
    {
        ArgumentNullException.ThrowIfNull(args);

        Args = args;
        FlagMappings = flagMappings ?? Array.Empty<string>();

        if (switchMappings != null)
        {
            _switchMappings = GetValidatedSwitchMappingsCopy(switchMappings);
        }
    }

    /// <summary>
    /// Gets the command-line arguments.
    /// </summary>
    protected IEnumerable<string> Args { get; }
    protected IEnumerable<string> FlagMappings { get; }

    /// <summary>
    /// Loads the configuration data from the command-line arguments.
    /// </summary>
    public override void Load()
    {
        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        string key, value;

        using (IEnumerator<string> enumerator = Args.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                var currentflag = FlagMappings.FirstOrDefault(f => f.Equals(enumerator.Current, StringComparison.OrdinalIgnoreCase));
                string currentArg = enumerator.Current;
                int keyStartIndex = 0;

                if (currentArg.StartsWith("--"))
                {
                    keyStartIndex = 2;
                }
                else if (currentArg.StartsWith("-"))
                {
                    keyStartIndex = 1;
                }
                else if (currentArg.StartsWith("/"))
                {
                    // "/SomeSwitch" is equivalent to "--SomeSwitch" when interpreting switch mappings
                    // So we do a conversion to simplify later processing
                    currentArg = $"--{currentArg.Substring(1)}";
                    keyStartIndex = 2;
                }

                int separator = currentArg.IndexOf('=');

                if (separator < 0)
                {
                    // If there is neither equal sign nor prefix in current argument, it is an invalid format
                    if (keyStartIndex == 0)
                    {
                        // Ignore invalid formats
                        continue;
                    }

                    // If the switch is a key in given switch mappings, interpret it
                    if (_switchMappings != null && _switchMappings.TryGetValue(currentArg, out string? mappedKey))
                    {
                        key = mappedKey;
                    }
                    // If the switch starts with a single "-" and it isn't in given mappings , it is an invalid usage so ignore it
                    else if (keyStartIndex == 1)
                    {
                        continue;
                    }
                    // Otherwise, use the switch name directly as a key
                    else
                    {
                        key = currentArg.Substring(keyStartIndex);
                    }

                    if (string.IsNullOrEmpty(currentflag) && !enumerator.MoveNext())
                    {
                        // ignore missing values
                        continue;
                    }

                    value = string.IsNullOrEmpty(currentflag) ? enumerator.Current : true.ToString();
                }
                else
                {
                    string keySegment = currentArg.Substring(0, separator);

                    // If the switch is a key in given switch mappings, interpret it
                    if (_switchMappings != null && _switchMappings.TryGetValue(keySegment, out string? mappedKeySegment))
                    {
                        key = mappedKeySegment;
                    }
                    // If the switch starts with a single "-" and it isn't in given mappings , it is an invalid usage
                    else if (keyStartIndex == 1)
                    {
                        throw new FormatException($"{currentArg} is used as a short parameter but is not used into the switch mappings");
                    }
                    // Otherwise, use the switch name directly as a key
                    else
                    {
                        key = currentArg.Substring(keyStartIndex, separator - keyStartIndex);
                    }

                    value = currentArg.Substring(separator + 1);
                }

                // Override value when key is duplicated. So we always have the last argument win.
                data[key] = value;
            }
        }

        Data = data;
    }

    private static Dictionary<string, string> GetValidatedSwitchMappingsCopy(IDictionary<string, string> switchMappings)
    {
        // The dictionary passed in might be constructed with a case-sensitive comparer
        // However, the keys in configuration providers are all case-insensitive
        // So we check whether the given switch mappings contain duplicated keys with case-insensitive comparer
        var switchMappingsCopy = new Dictionary<string, string>(switchMappings.Count, StringComparer.OrdinalIgnoreCase);
        foreach (KeyValuePair<string, string> mapping in switchMappings)
        {
            // Only keys start with "--" or "-" are acceptable
            if (!mapping.Key.StartsWith("-") && !mapping.Key.StartsWith("--"))
            {
                throw new ArgumentException(
                    $"{mapping.Key} is not valid. Only keys start with \"--\" or \"-\" are acceptable",
                    nameof(switchMappings));
            }

            if (switchMappingsCopy.ContainsKey(mapping.Key))
            {
                throw new ArgumentException(
                     $"{mapping.Key} is not valid. Duplicated switch keys are not supported.",
                    nameof(switchMappings));
            }

            switchMappingsCopy.Add(mapping.Key, mapping.Value);
        }

        return switchMappingsCopy;
    }
}


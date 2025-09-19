using Dataverse.ConfigurationMigrationTool.Console.ConfigurationProviders;
using Shouldly;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.ConfigurationProviders;

public class CustomCommandLineConfigurationProviderTests
{
    [Fact]
    public void Given_ArgsWithDoubleDash_When_Load_Then_ParsesKeyValueCorrectly()
    {
        // Arrange
        var args = new[] { "--key=value" };
        var provider = BuildProvider(args);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "key").ShouldBe("value");
    }
    [Fact]
    public void Given_ArgsWithDoubleDashWithNoSeparator_When_Load_Then_ParsesKeyValueCorrectly()
    {
        // Arrange
        var args = new[] { "--key", "value" };
        var provider = BuildProvider(args);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "key").ShouldBe("value");
    }
    [Fact]
    public void Given_ArgsWithSingleDashWithNoSeparator_When_Load_Then_ParsesKeyValueCorrectly()
    {
        // Arrange
        var args = new[] { "-key", "value" };
        var switchMappings = new Dictionary<string, string> { { "-key", "key" } };
        var provider = BuildProvider(args, switchMappings);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "key").ShouldBe("value");
    }
    [Fact]
    public void Given_ArgsWithSingleDash_When_Load_Then_ParsesKeyValueCorrectly()
    {
        // Arrange
        var args = new[] { "-key=value" };
        var switchMappings = new Dictionary<string, string> { { "-key", "key" } };
        var provider = BuildProvider(args, switchMappings);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "key").ShouldBe("value");
    }

    [Fact]
    public void Given_ArgsWithSlash_When_Load_Then_ParsesKeyValueCorrectly()
    {
        // Arrange
        var args = new[] { "/key=value" };
        var provider = BuildProvider(args);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "key").ShouldBe("value");
    }

    [Fact]
    public void Given_ArgsWithSwitchMappings_When_Load_Then_MapsSwitchToCustomKey()
    {
        // Arrange
        var args = new[] { "--customSwitch=value" };
        var switchMappings = new Dictionary<string, string> { { "--customSwitch", "mappedKey" } };
        var provider = BuildProvider(args, switchMappings);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "mappedKey").ShouldBe("value");
    }

    [Fact]
    public void Given_ArgsWithFlagMappings_When_Load_Then_SetsFlagValueToTrue()
    {
        // Arrange
        var args = new[] { "--flag" };
        var flagMappings = new[] { "--flag" };
        var provider = BuildProvider(args, null, flagMappings);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "flag").ShouldBe("True");
    }

    [Fact]
    public void Given_ArgsWithDuplicateKeys_When_Load_Then_LastValueWins()
    {
        // Arrange
        var args = new[] { "--key=first", "--key=second" };
        var provider = BuildProvider(args);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "key").ShouldBe("second");
    }

    [Fact]
    public void Given_ArgsWithInvalidFormat_When_Load_Then_IgnoresInvalidArgs()
    {
        // Arrange
        var args = new[] { "invalidArg", "--valid=value" };
        var provider = BuildProvider(args);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "invalidArg").ShouldBeNull();
        GetValue(provider, "valid").ShouldBe("value");
    }

    [Fact]
    public void Given_ShortDashWithoutMapping_When_Load_Then_IgnoresArg()
    {
        // Arrange
        var args = new[] { "-short" };
        var provider = BuildProvider(args);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "short").ShouldBeNull();
    }

    [Fact]
    public void Given_ShortDashWithMapping_When_Load_Then_MapsKey()
    {
        // Arrange
        var args = new[] { "-s=value" };
        var switchMappings = new Dictionary<string, string> { { "-s", "shortKey" } };
        var provider = BuildProvider(args, switchMappings);
        // Act
        provider.Load();
        // Assert
        GetValue(provider, "shortKey").ShouldBe("value");
    }

    [Fact]
    public void Given_ShortDashWithNoMappingAndEquals_When_Load_Then_ThrowsFormatException()
    {
        // Arrange
        var args = new[] { "-s=value" };
        var provider = BuildProvider(args);
        // Act
        var act = () => provider.Load();
        // Assert
        act.ShouldThrow<FormatException>();
    }

    [Fact]
    public void Given_SwitchMappingsWithInvalidKey_When_Construct_Then_ThrowsArgumentException()
    {
        // Arrange
        var switchMappings = new Dictionary<string, string> { { "invalidKey", "mappedKey" } };
        var args = new[] { "--key=value" };
        // Act
        var act = () => BuildProvider(args, switchMappings);
        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Given_SwitchMappingsWithDuplicateKeys_When_Construct_Then_ThrowsArgumentException()
    {
        // Arrange
        var switchMappings = new Dictionary<string, string>
        {
            { "--dup", "key1" },
            { "--DUP", "key2" }
        };
        var args = new[] { "--dup=value" };
        // Act
        var act = () => BuildProvider(args, switchMappings);
        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Given_ArgsIsNull_When_Construct_Then_ThrowsArgumentNullException()
    {
        // Act
        var act = () => BuildProvider(null!);
        //Assert
        act.ShouldThrow<ArgumentNullException>();
    }
    private static CustomCommandLineConfigurationProvider BuildProvider(IEnumerable<string> args, IDictionary<string, string>? switchMappings = null, IEnumerable<string>? flagMappings = null)
    {
        return new CustomCommandLineConfigurationProvider(args, switchMappings, flagMappings);
    }

    // Helper to get value from provider's TryGet method using reflection
    private static string? GetValue(CustomCommandLineConfigurationProvider provider, string key)
    {
        if (provider.TryGet(key, out var value))
        {
            return value;
        }
        return null;
    }
}

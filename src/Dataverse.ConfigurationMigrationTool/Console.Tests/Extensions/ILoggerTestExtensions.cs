using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Extensions;
internal static class ILoggerTestExtensions
{
    public static void ShouldHaveLoggedException(this ILogger logger, LogLevel logLevel, Exception exception)
    {
        logger.Received().Log(logLevel, Arg.Any<EventId>(), Arg.Any<object>(), exception, Arg.Any<Func<object, Exception, string>>());
    }
}

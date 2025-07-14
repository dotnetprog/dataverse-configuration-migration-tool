using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dataverse.ConfigurationMigrationTool.Console.Tests.Extensions;
internal static class ILoggerTestExtensions
{
    public static void ShouldHaveLoggedException(this ILogger logger, LogLevel logLevel, Exception exception)
    {
        logger.Received().Log(logLevel, Arg.Any<EventId>(), Arg.Any<object>(), exception, Arg.Any<Func<object, Exception, string>>());
    }
    public static void ShouldHaveLogged(this ILogger logger, LogLevel logLevel, string message, int count = 1)
    {
        logger.Received(count).Log(
            logLevel,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == message),
            null,
            Arg.Any<Func<object, Exception, string>>());
    }
}

// TODOS
// - response error messages handling
//     : catch errors in responses and write them to exception.json at corresponding directory
// - add unit tests
// - add tests/ build/ pack workflows
// - add user manual

using BingImageCreatorDotnet.Lib.Config;
using BingImageCreatorDotnet.Lib.Handlers;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static void Main() => MainAsync().GetAwaiter().GetResult();

    static async Task  MainAsync()
        => await new BingImageCreatorHandler(
            LoggerFactory
                .Create(
                    builder => builder
                        .AddConsole()
                        .AddDebug()
                        .SetMinimumLevel(LogLevel.Debug))
                .CreateLogger<BingImageCreatorHandler>(),
            new ConfigReader(),
            new ConfigMapper())
            .RunTask();
}
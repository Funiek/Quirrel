using Quirrel.Interfaces;
using ILoggerProvider = Quirrel.Interfaces.ILoggerProvider;

try
{
    ILoggerProvider loggerProvider = new FileLoggerProvider();
    Log.Logger = loggerProvider.Logger;

    IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddLogging(configure => configure.AddSerilog())
                .AddTransient<IGoogleApiHandler, GoogleApiHandler>();
    })
    .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal($"Unsupported exception: {ex.Message}");
}
finally
{
    Log.CloseAndFlush();
}


using ILogger = Serilog.ILogger;
using ILoggerProvider = Quirrel.Interfaces.ILoggerProvider;

namespace Quirrel.Services
{
    public class FileLoggerProvider : ILoggerProvider
    {
        public ILogger Logger { get; set; }
        public FileLoggerProvider()
        {
            Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void Information(string message)
        {
            Logger.Information(message);
        }
        public void Error(string message)
        {
            Logger.Error(message);
        }
        public void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }
    }
}

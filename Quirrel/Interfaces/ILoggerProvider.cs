using ILogger = Serilog.ILogger;

namespace Quirrel.Interfaces
{
    public interface ILoggerProvider
    {
        ILogger Logger { get; set; }

        public void Information(string message);
        public void Error (string message);

        public void Fatal (string message);
        public void Debug (string message);


    }
}

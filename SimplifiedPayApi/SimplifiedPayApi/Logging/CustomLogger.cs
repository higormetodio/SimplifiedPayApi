
namespace SimplifiedPayApi.Logging;

public class CustomLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";

        WriteTextFile(message);
    }

    public void WriteTextFile(string message)
    {
        string path = @"C:\dev\Projects\SimplifiedPayApi\Log\apilogger.txt";

        using StreamWriter streamWriter = new(path, true);
        streamWriter.WriteLine(message);
        streamWriter.Close();
    }
}

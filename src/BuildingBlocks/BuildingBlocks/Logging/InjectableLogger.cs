using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Logging
{
    public interface IInjectableLogger
    {
        void Inform(string message);
        void Error(string message, Exception ex);
        void Warning(string message);
        void Debug(string message);
    }

    public class InjectableLogger : IInjectableLogger
    {
        private readonly ILogger _logger;

        public InjectableLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Application") 
                ?? throw new ArgumentNullException(nameof(loggerFactory), "Failed to create logger");
        }

        public void Inform(string message) => WriteLog(LogLevel.Information, message);
        public void Error(string message, Exception ex) => WriteLog(LogLevel.Error, message, ex);
        public void Warning(string message) => WriteLog(LogLevel.Warning, message);
        public void Debug(string message) => WriteLog(LogLevel.Debug, message);

        private void WriteLog(LogLevel level, string content, Exception? ex = null)
        {
            // Giữ nguyên định dạng console như yêu cầu
            var logEntry = $"\n[{level}] {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n - {content}\n";
            Console.WriteLine(logEntry);

            _logger.Log(
                level,
                new EventId((int)DateTimeOffset.UtcNow.Ticks % 1000000, "CustomEvent"),
                ex,
                "{Content}",
                content
            );
        }
    }

    public static class LoggingExtensions
    {
        public static IServiceCollection AddInjectableLogger(this IServiceCollection services)
        {
            services.AddSingleton<IInjectableLogger, InjectableLogger>();
            return services;
        }
    }
} 
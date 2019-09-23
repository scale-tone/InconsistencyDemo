using Serilog;
using Serilog.Enrichers;
using Serilog.Formatting.Json;
using Serilog.Sinks.RollingFile;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace DemoService
{
    public class LogConfig
    {
        public static void ConfigureLogging()
        {
            const int retainedFileCountLimit = 31;

            var logDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var applicationName = Assembly.GetExecutingAssembly().GetName().Name;
            var logFileNameFormat = $"{applicationName}-{{Date}}.log";
            var pathFormat = Path.Combine(logDirectory, logFileNameFormat);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.With<ThreadIdEnricher>()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .WriteTo.ColoredConsole()
                .WriteTo.Sink(new RollingFileSink(pathFormat, new JsonFormatter(renderMessage: true), null, retainedFileCountLimit, Encoding.UTF8))
                .CreateLogger();
        }
    }
}

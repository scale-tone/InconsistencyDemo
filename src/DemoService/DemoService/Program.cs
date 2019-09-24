using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using NServiceBus.Serilog;
using System.Configuration;

namespace DemoService.Infrastructure
{
    public class Program
    {
        static void Main(string[] args)
        {
            LogConfig.ConfigureLogging();
            LogManager.Use<SerilogFactory>();

            var endpointName = ConfigurationManager.AppSettings["DemoService.NServiceBus.InputQueue"];
            var endpointConfiguration = new EndpointConfiguration(endpointName);

            endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.SendFailedMessagesTo($"{endpointName}.error");

            //endpointConfiguration.LimitMessageProcessingConcurrencyTo(int.Parse(ConfigurationManager.AppSettings["DemoService.NServiceBus.MaximumConcurrencyLevel"]));

            // Make sure the queues are created.
            endpointConfiguration.EnableInstallers();

            endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
            endpointConfiguration.DisableFeature<Sagas>();
            endpointConfiguration.DisableFeature<Audit>();
            endpointConfiguration.DisableFeature<TimeoutManager>();

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                customizations: immediate =>
                {
                    immediate.NumberOfRetries(0);
                });

            endpointConfiguration.Conventions().DefiningCommandsAs(t =>
            {
                var commands = t.Namespace != null && (t.Namespace.EndsWith("Commands"));
                return commands;
            });

            var endpointInstance = Endpoint.Start(endpointConfiguration);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}

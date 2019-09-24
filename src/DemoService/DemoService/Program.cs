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

            var busConfiguration = new BusConfiguration();

            busConfiguration.UseTransport<MsmqTransport>();
            busConfiguration.EndpointName(ConfigurationManager.AppSettings["DemoService.NServiceBus.InputQueue"]);

            // Make sure the queues are created.
            busConfiguration.EnableInstallers();

            busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
            busConfiguration.DisableFeature<Sagas>();
            busConfiguration.DisableFeature<Audit>();
            busConfiguration.DisableFeature<TimeoutManager>();
            busConfiguration.DisableFeature<SecondLevelRetries>();

            //busConfiguration.DiscardFailedMessagesInsteadOfSendingToErrorQueue();
            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.Disable();

            busConfiguration.Conventions().DefiningCommandsAs(t =>
            {
                var commands = t.Namespace != null && (t.Namespace.EndsWith("Commands"));
                return commands;
            });

            var startableBus = Bus.Create(busConfiguration);
            var bus = startableBus.Start();
            Console.ReadKey();
        }
    }
}

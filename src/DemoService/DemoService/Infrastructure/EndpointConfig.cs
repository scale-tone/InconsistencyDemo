using System;
using System.Configuration;
using DemoService.Commands;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using NServiceBus.Serilog;

namespace DemoService.Infrastructure
{
    class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            LogConfig.ConfigureLogging();
            LogManager.Use<SerilogFactory>();

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
        }
    }
}

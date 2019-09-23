using System.Configuration;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace DemoService.Infrastructure
{
    class TransportConfigProvider : IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            var maxConcurrencyLevelAppSetting = ConfigurationManager.AppSettings["DemoService.NServiceBus.MaximumConcurrencyLevel"];
            var maxRetriesAppSetting = ConfigurationManager.AppSettings["DemoService.NServiceBus.MaximumNumberOfRetries"];

            var maximumConcurrencyLevel = int.Parse(maxConcurrencyLevelAppSetting);
            var maxRetries = int.Parse(maxRetriesAppSetting);

            return new TransportConfig
            {
                MaximumConcurrencyLevel = maximumConcurrencyLevel,
                MaxRetries = maxRetries
            };
        }
    }
}

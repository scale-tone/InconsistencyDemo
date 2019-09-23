using System.Configuration;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace DemoService.Infrastructure
{
    public class MessageForwardingInCaseOfFaultConfigProvider :IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            var inputQueueAppSetting = ConfigurationManager.AppSettings["DemoService.NServiceBus.InputQueue"]; // "queuename@localhost"
            var inputQueueName = inputQueueAppSetting.Split('@')[0]; // queuename
            var errorQueueName =  $"{inputQueueName}.error"; // queuename.error

            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = errorQueueName
            };
        }
    }
}

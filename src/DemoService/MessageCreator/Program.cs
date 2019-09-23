using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DemoService.Commands;

namespace MessageCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageCount = 100;
            var queueName = ConfigurationManager.AppSettings["DemoService.NServiceBus.InputQueue"].Split('@')[0];

            var queueAddress = $".\\private$\\{queueName}";

            Console.WriteLine($"Press any key to send {messageCount} messages to {queueAddress}.");
            Console.ReadLine();

            SendMessages(messageCount,queueAddress);
        }

        public static void SendMessages(int count, string queueAddress)
        {
            using (var myQueue = new MessageQueue(queueAddress))
            {
                for (int i = 0; i < count; i++)
                {
                    using (var myTransaction = new MessageQueueTransaction())
                    {
                        myTransaction.Begin();
                        var workload = new DoStuff() {Message = Guid.NewGuid().ToString()};
                        myQueue.Send(workload, myTransaction);
                        myTransaction.Commit();
                    }
                }
            }
        }
    }
}

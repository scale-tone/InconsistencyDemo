using System;
using System.ServiceModel;

namespace WcfService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(Service1)))
            {
                host.Open();
                Console.WriteLine("Service Hosted Successfully");
                Console.Read();
            }
        }
    }
}

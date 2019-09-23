using System;
using System.ServiceModel;
using System.Transactions;

namespace WcfService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, TransactionIsolationLevel = IsolationLevel.ReadCommitted)]
    public class Service1 : IService1
    {
        [OperationBehavior(TransactionScopeRequired = true)]
        public void DoWork(string correlationId)
        {
            Console.WriteLine($"Time: {DateTime.Now}, Correlation: {correlationId}, Local: {Transaction.Current?.TransactionInformation.LocalIdentifier}, Distributed: {Transaction.Current?.TransactionInformation.DistributedIdentifier}, Status: {Transaction.Current?.TransactionInformation.Status}");
            throw new Exception("Dummy exception");
        }
    }
}

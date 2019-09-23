using System.ServiceModel;

namespace WcfService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        void DoWork(string correlationId);
    }
}

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using DemoService.WcfAgents;

namespace LoopHammer
{
    class Program
    {
        static void Main(string[] args)
        {
            var loopCount = 1000000;

            Parallel.For(0, loopCount, i =>
            {
                new DostuffHandler().Handle();
            });
        }
    }

    public class DostuffHandler 
    {
        private string correlationId;

        public void Handle()
        {
            correlationId = Guid.NewGuid().ToString();
            try
            {
                /*
                 * For legacy reasons, we want to handle the transaction ourselves.
                 */
                var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
                using (var transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    /*
                     * Since the above TransactionScope uses TransactionScopeOption.Required, we're in a new TransactionScope and @@trancount is 1 99.99% of the time
                     * Some times @@trancount is 0 and even more seldom it is 2.
                     */
                    var transactionDetails = GetTransactionDetails();

                    if (transactionDetails.TranCount != 1)
                    {
                        Console.WriteLine("{0} trandetails is {1}, WE'RE ABOUT TO CREATE INCONSISTENT DATA", correlationId, transactionDetails);
                    }

                    /*
                     * This writes to two different tables.
                     * - TestTable2 is written within the current transaction. When @@trancount is 0, the insert is NOT rolled back when the TransactionScope is rolled back.
                     * - TestTable3 is written to, Suppressing the transaction. This is basically just a log.
                     */
                    WriteImportantBusinessDataToDatabase($"This should not be committed ever, since the WCF call below always fails.", GetTransactionDetails());

                    /*
                     * This WCF call is transactional. The initial local transaction is promoted to a distributed transaction.
                     * It always throws exception, and in 99.9% of the messages handled, the insert in the method above is rolled back.
                     * But in some cases, the stuff is not rolled back. We can tell when that is about to happen, by looking at @@trancount. 
                     */
                    var serviceClient = new Service1Client("WSHttpBinding_IService1");
                    serviceClient.DoWork(correlationId);

                    WriteImportantBusinessDataToDatabase($"We never reach this location", GetTransactionDetails());

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                if (e.Message != "Dummy exception")
                {
                    Console.WriteLine("{0} {1}", correlationId, e.Message);
                }
            }
        }

        private TransactionDetails GetTransactionDetails()
        {
            var transactionDetails = new TransactionDetails();

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["InconsistencyDemo"].ConnectionString))
            {
                connection.Open();

                using (var sqlCommand = new SqlCommand($"select @@trancount as 'TranCount', XACT_STATE() as 'Xact', CURRENT_TRANSACTION_ID() as 'TranId'", connection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                transactionDetails.TranCount = (int)reader["TranCount"];
                                transactionDetails.XactState = (short)reader["Xact"];
                                transactionDetails.TranId = (long)reader["TranId"];
                            }
                        }
                    }
                }
            }

            return transactionDetails;
        }

        private void WriteImportantBusinessDataToDatabase(string message, TransactionDetails transactionDetails)
        {
            // This will be committed when "select @@trancount" is 0.
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["InconsistencyDemo"].ConnectionString))
            {
                connection.Open();
                var query = @"INSERT INTO [dbo].[ImportantBusinessData] ([Message], [CorrelationId]) VALUES (@message, @correlationId)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@message", $"{message}. Transaction details: {transactionDetails}");
                    command.Parameters.AddWithValue("@correlationId", correlationId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    internal struct TransactionDetails
    {
        public int TranCount { get; set; }
        public int XactState { get; set; }
        public long TranId { get; set; }

        public override string ToString()
        {
            return $"TranCount = {TranCount}, Xact = {XactState}, TranId = {TranId}";
        }
    }
}
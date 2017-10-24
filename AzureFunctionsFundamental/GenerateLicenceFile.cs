using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsFundamental
{
    public static class GenerateLicenceFile
    {
        [FunctionName("GenerateLicenceFile")]
        public static void Run([QueueTrigger("order")]Order order, TraceWriter log)
        {
            log.Info($"Received an order: Order {order.OrderId}, product {order.ProductId}, email {order.Email}");
        }
    }
}

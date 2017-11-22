using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsFundamental
{
    public static class OnPaymentReceived
    {
        [FunctionName("OnPaymentReceived")]
        public static async Task<object> Run(
            [HttpTrigger("post", WebHookType = "genericJson", Route = "newpurchase")]HttpRequestMessage req, 
            TraceWriter log,
            [Queue("order")] IAsyncCollector<Order> orderQueue,
            [Table("Order")] IAsyncCollector<Order> orderTable)
        {
            log.Info("Order Received");
            var order = await req.Content.ReadAsAsync<Order>();
            //var order = JsonConvert.DeserializeObject<Order>(jsonContent);
            log.Info($"Order {order.OrderId} received from {order.Email} for product {order.ProductId}");
            await orderQueue.AddAsync(order);

            order.PartitionKey = "Orders";
            order.RowKey = order.OrderId;
            await orderTable.AddAsync(order);

            return req.CreateResponse(HttpStatusCode.OK,
                new
                {
                    message = "thank you for your order"
                });
        }
    }
}

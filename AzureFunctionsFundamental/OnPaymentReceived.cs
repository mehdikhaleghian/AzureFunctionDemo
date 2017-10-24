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
            [Queue("order")] IAsyncCollector<Order> orderQueue)
        {
            log.Info("Order Received");
            var jsonContent = await req.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<Order>(jsonContent);
            log.Info($"Order {order.OrderId} received from {order.Email} for product {order.ProductId}");
            await orderQueue.AddAsync(order);
            return req.CreateResponse(HttpStatusCode.OK,
                new
                {
                    message = "thank you for your order"
                });
        }
    }
}

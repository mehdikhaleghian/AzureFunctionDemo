using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsFundamental
{
    public static class GenerateLicenceFile
    {
        [FunctionName("GenerateLicenceFile")]
        public static void Run([QueueTrigger("order")]Order order, TraceWriter log,
            /*[Blob("licences/{rand-guid}.lic")] TextWriter outputBlob*/
            IBinder binder)
        {
            log.Info($"Received an order: Order {order.OrderId}, product {order.ProductId}, email {order.Email}");
            using (var outputBlob = binder.Bind<TextWriter>( new BlobAttribute($"licences/{order.OrderId}.lic")))
            {
                outputBlob.WriteLine($"OrderId: {order.OrderId}");
                outputBlob.WriteLine($"Email: {order.Email}");
                outputBlob.WriteLine($"ProductId: {order.ProductId}");
                outputBlob.WriteLine($"Purchase Date: {DateTime.UtcNow}");
                var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(order.Email + "secret"));
                outputBlob.WriteLine($"SecretCode: {BitConverter.ToString(hash).Replace("-", "")}");
            }
        }
    }
}

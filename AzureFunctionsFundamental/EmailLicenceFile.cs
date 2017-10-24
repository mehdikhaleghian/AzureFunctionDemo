using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsFundamental
{
    public static class EmailLicenceFile
    {
        [FunctionName("EmailLicenceFile")]
        public static void Run([BlobTrigger("licences/{fileName}.lic")]Stream myBlob, string fileName, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{fileName} \n Size: {myBlob.Length} Bytes");
        }
    }
}

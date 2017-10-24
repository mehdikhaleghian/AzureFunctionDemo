using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsFundamental
{
    public static class EmailLicenceFile
    {
        [FunctionName("EmailLicenceFile")]
        public static void Run([BlobTrigger("licences/{fileName}.lic")]string myBlob, string fileName, TraceWriter log)
        {
            var email = Regex.Match(myBlob, @"^Email\:\ (.+)$", RegexOptions.Multiline).Groups[1].Value.Trim();
            log.Info($"Got order from {email}\n Lincence file name: {fileName}");
        }
    }
}

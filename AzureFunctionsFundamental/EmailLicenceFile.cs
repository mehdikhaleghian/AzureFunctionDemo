using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid.Helpers.Mail;

namespace AzureFunctionsFundamental
{
    public static class EmailLicenceFile
    {
        [FunctionName("EmailLicenceFile")]
        public static void Run([BlobTrigger("licences/{fileName}.lic")]string myBlob, string fileName, TraceWriter log,
            [SendGrid] out Mail message)
        {
            var email = Regex.Match(myBlob, @"^Email\:\ (.+)$", RegexOptions.Multiline).Groups[1].Value.Trim();
            log.Info($"Got order from {email}\n Lincence file name: {fileName}");

            message = new Mail();
            var personalization = new Personalization();
            personalization.AddTo(new Email(email));
            message.AddPersonalization(personalization);

            Attachment attachment = new Attachment();
            var plainTextBytes = Encoding.UTF8.GetBytes(myBlob);
            attachment.Content = Convert.ToBase64String(plainTextBytes);
            attachment.Type = "text/plain";
            attachment.Filename = "licence.lic";
            attachment.Disposition = "attachment";
            attachment.ContentId = "Licence File";
            message.AddAttachment(attachment);

            var messageContent = new Content("text/html", "Your licence file is attached");
            message.AddContent(messageContent);

            message.Subject = "Thank you for your order";

            message.From = new Email(Environment.GetEnvironmentVariable("AzureWebJobsFromEmailAddress"));
        }
    }
}

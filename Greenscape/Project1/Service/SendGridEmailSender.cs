using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using static System.Net.WebRequestMethods;

namespace Project1.Service
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly string _apiKey;

        public SendGridEmailSender(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(_apiKey);
            var msg = new SendGridMessage
            {
                /* From = new EmailAddress("greenscape@gardener.com", "Green Scape"),*/
               
               From = new EmailAddress("info@greenscape.life", "Greenscape"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            await client.SendEmailAsync(msg);
        }
    }
}

using Microsoft.Extensions.Logging;
using MimeKit;
using ShopAdmin.Services;

namespace ShopAdmin.Commands
{
    public class Manufacturer : ConsoleAppBase
    {
        private readonly ILogger<Manufacturer> _logger;
        private readonly IEmailService _emailService;

        public Manufacturer(ILogger<Manufacturer> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }
        public void Sendreport()
        {
            //if (DateTime.Now.Day != 3)
            //{
            //    _logger.LogInformation("No reports today.");
            //    return;
            //}
            _logger.LogInformation("Send report starting.");

            _emailService.Connect();
            _emailService.Authenticate();

            var sender = _emailService.GetSenderDetails();
            
            foreach (var manufacturer in _emailService.GetManufacturers())
            {
                var message = new MimeMessage();
                message.Subject = "Sales report";
                message.Body = new TextPart("plain") { Text = "Here is your sales report for the previous month:" };
                message.From.Add(sender);
                message.To.Add(new MailboxAddress(manufacturer.Name, manufacturer.EmailReport.Replace(" ", "")));
                _emailService.Send(message);
            }
            
            _emailService.Disconnect();
            _logger.LogInformation("Send report ending.");
        }
    }
}

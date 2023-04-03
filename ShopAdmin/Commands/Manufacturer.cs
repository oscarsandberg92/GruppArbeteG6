using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using ShopGeneral.Data;


namespace ShopAdmin.Commands
{
    public class Manufacturer : ConsoleAppBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly SmtpClient _client;

        public Manufacturer(ApplicationDbContext context, ILogger<ApplicationDbContext> logger)
        {
            _context = context;
            _logger = logger;
            _client = new SmtpClient();
        }
        public void Sendreport()
        {
            if (DateTime.Now.Day != 3)
            {
                _logger.LogInformation("No reports today.");
                return;
            }
            _logger.LogInformation("Send report starting.");
            _client.Connect("smtp.ethereal.email", 587, false);
            _client.Authenticate("laisha.cormier@ethereal.email", "vH6xp64c7dPYhCyXwY");
            var sender = new MailboxAddress("Grupp6", "Grupp6@tuc.se");
            foreach (var manufacturer in _context.Manufacturers)
            {
                var message = new MimeMessage();
                message.Subject = "Sales report";
                message.Body = new TextPart("plain") { Text = "Here is your sales report for the previous month:" };
                message.From.Add(sender);
                message.To.Add(new MailboxAddress(manufacturer.Name, manufacturer.EmailReport.Replace(" ", "")));
                _client.Send(message);
            }
            _client.Disconnect(true);
            _logger.LogInformation("Send report ending.");
        }
    }
}

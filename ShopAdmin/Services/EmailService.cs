using MailKit.Net.Smtp;
using MimeKit;
using ShopGeneral.Data;

namespace ShopAdmin.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _client;
        private readonly ApplicationDbContext _context;

        private readonly string host = "smtp.ethereal.email";
        private readonly int port = 587;
        private readonly bool useSsl = false;
        private readonly string userName = "laisha.cormier@ethereal.email";
        private readonly string password = "vH6xp64c7dPYhCyXwY";


        public EmailService(ApplicationDbContext context)
        {
            _context = context;
            _client = new SmtpClient();
        }

        public void Authenticate()
        {
            _client.Authenticate(userName, password);
        }

        public void Connect()
        {
            _client.Connect(host, port, useSsl);
        }

        public void Disconnect()
        {
            _client.Disconnect(true);
        }

        public List<ShopGeneral.Data.Manufacturer> GetManufacturers()
        {
            return _context.Manufacturers.ToList();
        }

        public MailboxAddress GetSenderDetails()
        {
            return new MailboxAddress("Grupp 6", "grupp6@tuc.se");
        }

        public void Send(MimeMessage message)
        {
            _client.Send(message);
        }
    }
}

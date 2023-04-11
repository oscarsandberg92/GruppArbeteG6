using MimeKit;

namespace ShopAdmin.Services
{
    public interface IEmailService
    {
        public void Send(MimeMessage message);

        void Disconnect();

        void Connect();

        void Authenticate();

        MailboxAddress GetSenderDetails();

        List<ShopGeneral.Data.Manufacturer> GetManufacturers();
    }
}

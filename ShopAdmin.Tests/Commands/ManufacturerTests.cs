using Microsoft.Extensions.Logging;
using Moq;
using ShopAdmin.Commands;
using MimeKit;
using ShopAdmin.Services;

namespace ShopAdmin.Tests.Commands
{
    [TestClass]
    public class ManufacturerTests
    {

        private Mock<ILogger<Manufacturer>> _mockLogger;
        private Manufacturer _sut;
        private readonly Mock<IEmailService> _mockEmailService;

        public ManufacturerTests()
        {
            _mockEmailService= new Mock<IEmailService>();
            _mockLogger = new Mock<ILogger<Manufacturer>>();
            _sut = new Manufacturer(_mockLogger.Object, _mockEmailService.Object);
        }

        [TestMethod]
        public void When_sending_report_send_should_be_called_once_for_each_manufacturer()
        {
            // Arrange
            List<ShopGeneral.Data.Manufacturer> manufacturers = new();
            
            manufacturers.Add(new()
            {
                Name = "Testnamn1",
                EmailReport = "Testemail@test.com",
                Id = 1,
                Icon = "1"
            });

            MailboxAddress sender = new("testsender", "testsender@test.com");

            _mockEmailService.Setup( s => s.GetManufacturers()).Returns(manufacturers);
            _mockEmailService.Setup(s => s.GetSenderDetails()).Returns(sender);

            // Act
            _sut.Sendreport();

            // Assert
            _mockEmailService.Verify(x => x.Send(It.IsAny<MimeMessage>()), Times.Once());
        }
    }
}

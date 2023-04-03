using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using ShopAdmin.Model;
using ShopGeneral.Services;

namespace ShopAdmin.Tests.Commands
{
    [TestClass]
    public class ProductTests
    {
        private ShopAdmin.Commands.Product _sut;
        private Mock<ILogger<ProductServiceModel>> _mockLogger;
        private Mock<IProductService> _mockProductService;

        public ProductTests()
        {
            _mockProductService = new();
            _mockLogger = new Mock<ILogger<ProductServiceModel>>();
            _sut = new ShopAdmin.Commands.Product(_mockLogger.Object, _mockProductService.Object);
        }

        [TestMethod]
        public void When_exporting_products_file_should_exist()
        {
            // Arrange
            string fileName = DateTime.Now.ToShortDateString().Replace("-", "");

            List<ProductServiceModel> list = new()
            {
                new ProductServiceModel { Name = "Test1" }
            };

            _mockProductService.Setup(s => s.GetAllProducts()).Returns(list);

            // Act
            _sut.Export("pricerunner");

            // Assert
            Assert.IsTrue(File.Exists($"outfiles\\pricerunner\\{fileName}.txt"));
        }
        [TestMethod]
        public void When_exporting_products_all_products_should_be_in_the_file()
        {
            // Arrange
            string fileName = DateTime.Now.ToShortDateString().Replace("-", "");

            List<ProductServiceModel> list = new()
            {
                new ProductServiceModel { Name = "Test1" },
                new ProductServiceModel { Name = "Test2" },
                new ProductServiceModel { Name = "Test3" }
            };

            _mockProductService.Setup(s => s.GetAllProducts()).Returns(list);

            // Act
            _sut.Export("pricerunner");

            string productsFromFile = File.ReadAllText($"outfiles\\\\pricerunner\\\\{fileName}.txt");
            ProductListExportModel result = JsonConvert.DeserializeObject<ProductListExportModel>(productsFromFile);

            // Assert
            Assert.AreEqual(list.Count, result.ProductList.Count);
        }
        [TestMethod]
        public void When_verifying_image_file_should_exist()
        {
            // Arrange
            string dateString = DateTime.Now.ToShortDateString().Replace("-", "");
            string fileName = $"missingimages-{dateString}.txt";

            List<ProductServiceModel> list = new()
            {
                new ProductServiceModel { Name = "Test1", ImageUrl ="http://www.google.com" , Id = 1},
                new ProductServiceModel { Name = "Test2", ImageUrl ="HokusPokus", Id = 2},
            };

            _mockProductService.Setup(s => s.GetAllProducts()).Returns(list);

            // Act
            _sut.Verifyimage();

            // Assert
            Assert.IsTrue(File.Exists($"outfiles\\products\\{fileName}"));
        }
        [TestMethod]
        public void When_verifying_image_file_should_contain_all_ids_that_are_missing_image()
        {
            // Arrange
            string dateString = DateTime.Now.ToShortDateString().Replace("-", "");
            string fileName = $"missingimages-{dateString}.txt";

            List<ProductServiceModel> list = new()
            {
                new ProductServiceModel { Name = "Test1", ImageUrl ="http://www.google.com" , Id = 1},
                new ProductServiceModel { Name = "Test2", ImageUrl ="HokusPokus", Id = 2},
            };

            _mockProductService.Setup(s => s.GetAllProducts()).Returns(list);

            // Act
            _sut.Verifyimage();
            var lines = File.ReadLines($"outfiles\\products\\{fileName}");

            List<int> productIds = new();
            foreach(string line in lines)
            {
                productIds.Add(Convert.ToInt32(line));
            }

            // Assert
            Assert.AreEqual(2, productIds[0]);
        }
    }
}

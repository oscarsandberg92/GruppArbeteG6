using Microsoft.Extensions.Logging;
using Moq;
using ShopGeneral.Services;


namespace ShopAdmin.Tests.Commands
{
    [TestClass]
    public class CategoryTests
    {
        private Mock<ILogger<ShopGeneral.Data.Category>> _mockLogger;
        private Mock<IProductService> _mockProductService;
        private Mock<ICategoryService> _mockCategoryService;
        private ShopAdmin.Commands.Category _sut;

        public CategoryTests()
        {
            _mockCategoryService= new Mock<ICategoryService>();
            _mockProductService= new Mock<IProductService>();
            _mockLogger= new Mock<ILogger<ShopGeneral.Data.Category>>();
            _sut = new ShopAdmin.Commands.Category(
                _mockLogger.Object,
                _mockProductService.Object,
                _mockCategoryService.Object
                );
        }

        [TestMethod]
        public void when_check_empty_file_should_exist()
        {
            // Arrange
            string dateString = DateTime.Now.ToShortDateString().Replace("-", "");
            string fileName = $"missingproducts-{dateString}.txt";

            List<ShopGeneral.Data.Category> categories = new();
            categories.Add(new ShopGeneral.Data.Category { Name = "TestCategory1", Id = 1, Icon = "testIcon" });
            categories.Add(new ShopGeneral.Data.Category { Name = "TestCategory2", Id = 2, Icon = "testIcon2" });


            List<ProductServiceModel> products = new();
            products.Add(new ProductServiceModel { CategoryName = "TestCategory1" });

            _mockProductService.Setup(s => s.GetAllProducts()).Returns(products);
            _mockCategoryService.Setup(s => s.GetAllCategories()).Returns(categories);

            // Act
            _sut.Checkempty();

            // Assert
            Assert.IsTrue(File.Exists($"outfiles\\category\\{fileName}"));
        }

        [TestMethod]
        public void when_check_empty_file_should_contain_all_empty_categories()
        {
            // Arrange
            string dateString = DateTime.Now.ToShortDateString().Replace("-", "");
            string fileName = $"missingproducts-{dateString}.txt";
            string expectedCategoryName = "TestCategory2";

            List<ShopGeneral.Data.Category> categories = new();
            categories.Add(new ShopGeneral.Data.Category { Name = "TestCategory1", Id = 1, Icon = "testIcon" });
            categories.Add(new ShopGeneral.Data.Category { Name = expectedCategoryName, Id = 2, Icon = "testIcon2" });


            List<ProductServiceModel> products = new();
            products.Add(new ProductServiceModel { CategoryName = "TestCategory1" });

            _mockProductService.Setup(s => s.GetAllProducts()).Returns(products);
            _mockCategoryService.Setup(s => s.GetAllCategories()).Returns(categories);

            // Act
            _sut.Checkempty();
            var emptyCategoryNames = File.ReadLines($"outfiles\\category\\{fileName}").ToList();
            // Assert
            Assert.AreEqual(expectedCategoryName, emptyCategoryNames[0]);
        }

    }
}

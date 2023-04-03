using Microsoft.Extensions.Logging;
using Moq;
using ShopAdmin.Commands;
using ShopGeneral.Data;
using ShopGeneral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

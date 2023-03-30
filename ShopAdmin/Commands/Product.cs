using Microsoft.Extensions.Logging;
using ShopAdmin.Model;
using ShopGeneral.Services;
using Newtonsoft.Json;

namespace ShopAdmin.Commands
{
    public class Product : ConsoleAppBase
    {
        private readonly ILogger<ProductServiceModel> _logger;
        private readonly IProductService _productService;
        private readonly string filePath;

        public Product(ILogger<ProductServiceModel> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
            string dateString = DateTime.Now.ToShortDateString().Replace("-", "");
            filePath = $"..\\..\\..\\outfiles\\pricerunner\\{dateString}.txt";
        }

        public void Export(string to)
        {
            _logger.LogInformation("Export starting");
            var products = _productService.GetAllProducts();

            ProductListExportModel result = new();

            foreach(ProductServiceModel p in products)
            {
                var newProduct = new ProductExportModel();
                newProduct.Id = p.Id;
                newProduct.Title = p.Name;
                newProduct.Price = p.BasePrice;
                newProduct.DiscountPercentage = 0;
                newProduct.Brand = p.ManufacturerName;
                newProduct.Category = p.CategoryName;
                newProduct.ImageUrl = p.ImageUrl;

                result.ProductList.Add(newProduct);
            }

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            File.WriteAllText(filePath, json);

            _logger.LogInformation("Export ending");
        }
    }
}

using Microsoft.Extensions.Logging;
using ShopAdmin.Model;
using ShopGeneral.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace ShopAdmin.Commands
{
    public class Product : ConsoleAppBase
    {
        private readonly ILogger<ProductServiceModel> _logger;
        private readonly IProductService _productService;
        private readonly HttpClient client;
    public Product(ILogger<ProductServiceModel> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
            client = new HttpClient();
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

            string directoryPath = $"outfiles\\{to}\\";
            Directory.CreateDirectory(directoryPath);

            string dateString = DateTime.Now.ToShortDateString().Replace("-", "");
            string filePath = $"{directoryPath}{dateString}.txt";
            File.WriteAllText(filePath, json);

            _logger.LogInformation("Export ending");
        }

        public void Verifyimage()
        {
            _logger.LogInformation("Verify image starting");
            var products = _productService.GetAllProducts();
            List<string> productIdMissingImage= new List<string>();
            foreach (var p in products)
            {
                var result = IsImageUrlValid(p.ImageUrl);
                if (!result.Result)
                {
                    productIdMissingImage.Add(p.Id.ToString());
                }
            }

            string directoryPath = "outfiles\\products\\";
            Directory.CreateDirectory(directoryPath);

            string dateString = DateTime.Now.ToShortDateString().Replace("-", "");
            string filePath = $"{directoryPath}missingimages-{dateString}.txt";

            File.WriteAllLines(filePath, productIdMissingImage);

            _logger.LogInformation("Verify image ending");
        }

        private async Task <bool> IsImageUrlValid(string imageUrl)
        {
            try
            {
                using HttpResponseMessage response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

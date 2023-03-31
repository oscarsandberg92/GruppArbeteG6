using Microsoft.Extensions.Logging;
using ShopGeneral.Services;

namespace ShopAdmin.Commands
{
    public class Category : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;
        private readonly string filePath;
        private readonly string directoryPath;

        public Category(ILogger<ShopGeneral.Data.Category> logger, IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
            string dateString = DateTime.Now.ToShortDateString().Replace("-", "");
            directoryPath = "outfiles\\category\\";
            filePath = $"{directoryPath}missingproducts-{dateString}.txt";
        }
         
        public void Checkempty()
        {
            _logger.LogInformation("CheckEmpty starting.");
            var categories = _categoryService.GetAllCategories();
            var products = _productService.GetAllProducts();

            Dictionary<string, int>categoryDictionary = new Dictionary<string, int>();

            foreach (var category in categories)
            {
                if(!categoryDictionary.ContainsKey(category.Name))
                {
                    categoryDictionary.Add(category.Name, 0);
                }
            }

            foreach (var product in products)
            {
                categoryDictionary[product.CategoryName] ++;
            }

            List<string> emptyCategories = new();
            foreach(var category in categoryDictionary)
            {
                if(category.Value == 0)
                    emptyCategories.Add(category.Key);
            }

            Directory.CreateDirectory(directoryPath);
            File.WriteAllLines(filePath, emptyCategories);
            _logger.LogInformation("CheckEmpty ending.");
        }
    }
}

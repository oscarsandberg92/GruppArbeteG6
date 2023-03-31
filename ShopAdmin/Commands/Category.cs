using Microsoft.Extensions.Logging;
using ShopGeneral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAdmin.Commands
{
    public class Category : ConsoleAppBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;


        public Category(ILogger<ProductServiceModel> logger, IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _logger = logger;
            _categoryService = categoryService;
        }
         
        public void Checkempty()
        {
            var categories = _categoryService.GetAllCategories();
            var products = _productService.GetAllProducts();

            Dictionary<string, int>categoryDictionary = new Dictionary<string, int>();

            foreach (var category in categories)
            {
                categoryDictionary.Add(category.Name, 0);
            }
            foreach (var product in products)
            {
                categoryDictionary[product.Name] ++;
            }

        }


    }
}

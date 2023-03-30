namespace ShopAdmin.Model
{
    public class ProductListExportModel
    {
        public List<ProductExportModel> ProductList { get; set; }
        public int Total { get { return ProductList.Count; }  }

        public ProductListExportModel()
        {
            ProductList = new List<ProductExportModel>();
        }
    }
}

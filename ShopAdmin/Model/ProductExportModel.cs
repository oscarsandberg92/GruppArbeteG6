namespace ShopAdmin.Model
{
    public class ProductExportModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double DiscountPercentage{ get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }
}

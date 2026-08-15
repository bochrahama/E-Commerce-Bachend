
namespace EcommerceBackend.Entities
{
    public class Product
    {
        
        public int ProductId { get; private set; }
       
        public string ProductName { get; set; } = string.Empty;
        public string? ProductDescription { get; set; } 
        public decimal ProductPrice { get; set; } 
        public decimal? ProductQuantity { get; set; }
        public  DateTime ProductCreatedAt { get; set; } = DateTime.UtcNow;
        public bool ProductIsActive { get; set; } = true;
        public Category ProductCategory { get; set; } 
        public int ProductCategoryId { get; set; }
        public ICollection <ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public Product() { }


    }
}

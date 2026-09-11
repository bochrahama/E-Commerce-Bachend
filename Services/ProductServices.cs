using Microsoft.EntityFrameworkCore;
using EcommerceBackend.Data;
using EcommerceBackend.Entities;
namespace EcommerceBackend.Services
{
    public class ProductServices : IProductService
    {
        private readonly AppDbContext _context;

        public ProductServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync(string? searchTerm , int? categoryId , decimal? minPrice , decimal? maxPrice )
        {
            var query = _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductImages)
                .Where(p => p.ProductIsActive)
                .AsQueryable();
             if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.ProductCategoryId == categoryId.Value);
            }
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.ProductPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.ProductPrice <= maxPrice.Value);
            }
            return await query.ToListAsync();
        }
        // the method below is used to get a product by its id and include its category and images
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductImages)
                // filter the product by its id and check if it is active
                .FirstOrDefaultAsync(p => p.ProductId == id && p.ProductIsActive);
        }
        //the method below is used to create a new product 
        // it takes a product object as a parameter and adds it to the database
        public async Task<Product> CreatAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();//save in database
            return product;
        }
        public async Task<bool> UpdateAsync(Product product)
        {
            // find the existing product by its id by using the FindAsync method of the DbContext
            var existingProduct = await _context.Products.FindAsync(product.ProductId);
            if (existingProduct == null || !existingProduct.ProductIsActive)
            {
                return false;
            }
            // update the existing product with the new values
            // update the existing product with the new values
            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductDescription = product.ProductDescription;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.ProductQuantity = product.ProductQuantity;
            existingProduct.ProductCategoryId = product.ProductCategoryId;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null || !existingProduct.ProductIsActive)
            {
                return false;
            }
            // mark the product as inactive instead of deleting it from the database
            existingProduct.ProductIsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        // the method below is used to reduce the stock of a product by its id and quantity
        //by checking if the product exists and is active and has enough quantity
        public async Task<bool> ReduceStockAsync(int productId, decimal quantity)
        {
            var existingProduct = await _context.Products.FindAsync(productId);
            if (existingProduct == null || !existingProduct.ProductIsActive || existingProduct.ProductQuantity < quantity)
            {
                return false;
            }// reduce the stock of the product by the quantity and save the changes to the database
            existingProduct.ProductQuantity -= quantity;
            await _context.SaveChangesAsync();
            return true;
        }
        }
}

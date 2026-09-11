//build the interface for the product service
using EcommerceBackend.Entities;
namespace EcommerceBackend.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync (string? search , int? categoryId , decimal? minPrice,decimal?maxPrice);
        Task<Product?> GetByIdAsync(int id);
        Task<Product>CreatAsync(Product product);
 
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        
        Task <bool> ReduceStockAsync (int productId, decimal quantity);
    }
}

using EcommerceBackend.Entities;
namespace EcommerceBackend.Services
{
    
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CtreatAsync(Category category);
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}

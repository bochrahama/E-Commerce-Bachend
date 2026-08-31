using EcommerceBackend.Repositories;
using EcommerceBackend.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualBasic;

namespace EcommerceBackend.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IGenericRepository<Category, int> _repo;
        public CategoryService(IGenericRepository<Category, int> repo) { _repo = repo; }
        public Task<IEnumerable<Category>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Category?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public async Task<Category> CtreatAsync(Category category)
        {
            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();
            return category;

        }
        public async Task<bool> UpdateAsync(Category category)
        {
            _repo.Update(category);
            return await _repo.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id) {
            var category = await _repo.GetByIdAsync(id);
            if (category is null) return false;
            _repo.Remove(category);
            return await _repo.SaveChangesAsync();
        }
        

} }
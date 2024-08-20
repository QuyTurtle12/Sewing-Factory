using SewingFactory.Models;
using SewingFactory.Models.DTOs;

namespace SewingFactory.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<object>> GetCategoriesAsync(int pageNumber, int pageSize);
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<bool> CategoryExistsAsync(Guid id);
        Task<Category> CreateCategoryAsync(CategoryDTO categoryDTO);
        System.Threading.Tasks.Task UpdateCategoryAsync(Guid id, CategoryDTO categoryDTO);
        System.Threading.Tasks.Task DeleteCategoryAsync(Guid id);
        Task<IEnumerable<object>> SearchCategory(int pageNumber, int pageSize, string searchTerm);
    }
}
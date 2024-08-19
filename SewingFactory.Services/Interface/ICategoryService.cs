using SewingFactory.Models.DTOs;
using SewingFactory.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SewingFactory.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<bool> CategoryExistsAsync(Guid id);
        Task<Category> CreateCategoryAsync(CategoryDTO categoryDTO);
        System.Threading.Tasks.Task UpdateCategoryAsync(Guid id, CategoryDTO categoryDTO);
        System.Threading.Tasks.Task DeleteCategoryAsync(Guid id);
    }
}

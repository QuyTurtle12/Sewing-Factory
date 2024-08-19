using Microsoft.EntityFrameworkCore;
using SewingFactory.Models.DTOs;
using SewingFactory.Models.Models;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SewingFactory.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DatabaseContext _context;

        public CategoryService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> CategoryExistsAsync(Guid id)
        {
            return await _context.Categories.AnyAsync(e => e.ID == id);
        }

        public async Task<Category> CreateCategoryAsync(CategoryDTO categoryDTO)
        {
            if (string.IsNullOrWhiteSpace(categoryDTO.Name))
            {
                throw new ArgumentException("Name is required and cannot be empty or whitespace.");
            }

            var categoryNameExists = await _context.Categories.AnyAsync(c => c.Name.Equals(categoryDTO.Name));
            if (categoryNameExists)
            {
                throw new ArgumentException("This category name already exists.");
            }

            Category newCategory = new Category(categoryDTO.Name);
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return newCategory;
        }

        public async System.Threading.Tasks.Task UpdateCategoryAsync(Guid id, CategoryDTO categoryDTO)
        {
            if (string.IsNullOrWhiteSpace(categoryDTO.Name))
            {
                throw new ArgumentException("Name is required and cannot be empty or whitespace.");
            }

            var categoryNameExists = await _context.Categories.AnyAsync(c => c.Name.Equals(categoryDTO.Name));
            if (categoryNameExists)
            {
                throw new ArgumentException("This category name already exists.");
            }

            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            category.Name = categoryDTO.Name;
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;
using SewingFactory.Models.DTOs;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;

namespace SewingFactory.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly DatabaseContext _context;

        public CategoryService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetCategoriesAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 0 || pageSize < 0) //return all
            {
                return await _context.Categories
                                 .Select(c => new { c.ID, c.Name })
                                 .ToListAsync();
            }
            else
            {
                return await _context.Categories
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .Select(c => new { c.ID, c.Name })
                                 .ToListAsync();
            }
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID '{id}' not found.");
            }

            return category;
        }

        public async Task<bool> CategoryExistsAsync(Guid id)
        {
            return await _context.Categories.AnyAsync(e => e.ID == id);
        }

        public async Task<Category> CreateCategoryAsync(CategoryDto categoryDTO)
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

        public async System.Threading.Tasks.Task UpdateCategoryAsync(Guid id, CategoryDto categoryDTO)
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

        public async Task<IEnumerable<object>> SearchCategory(int pageNumber, int pageSize, string searchTerm)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.ToUpper().Contains(searchTerm.ToUpper()));
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new { c.ID, c.Name })
                .ToListAsync();
        }
    }
}
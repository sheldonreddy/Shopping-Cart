using Microsoft.EntityFrameworkCore;
using GG_shopping_cart.Data;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Mappers;
using GG_shopping_cart.Entities;

namespace GG_shopping_cart.Services.Implementations
{
    public class CategoryService : ICategoryService, IAutoRegister
    {
        private readonly ProductDbContext _context;

        public CategoryService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _context.Categories.Select(category =>
            ModelConverter.ModelToDto(category)).ToListAsync();

            return categories;
        }

        public async Task<CategoryDto?> GetCategory(string id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id.ToString() == id);
            if (category == null)
            {
                return null;
            }

            return ModelConverter.ModelToDto(category);
        }


        public async Task<CategoryDto> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = ModelConverter.DtoToModel(categoryDto);

            _context.Categories.Update(category);
            
            await _context.SaveChangesAsync();

            return ModelConverter.ModelToDto(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryBaseDto categoryDto)
        {
            var category = ModelConverter.DtoToModel(categoryDto);

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return ModelConverter.ModelToDto(category);
        }


        public async Task<bool> DeleteCategory(string id)
        {
            var Category = await _context.Categories.FirstOrDefaultAsync(p => p.Id.ToString() == id);
            if (Category == null)
            {
                return false;
            }
            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();

            return true;

        }
    }
}


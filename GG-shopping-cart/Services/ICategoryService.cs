
namespace GG_shopping_cart.Services
{
    public interface ICategoryService
    {
        Task<bool> DeleteCategory(string id);

        Task<CategoryDto?> GetCategory(string id);

        Task<IEnumerable<CategoryDto>> GetAllCategories();

        Task<CategoryDto> UpdateCategoryAsync(CategoryDto categoryDto);

        Task<CategoryDto> CreateCategoryAsync(CategoryBaseDto categoryDto);

    }
}

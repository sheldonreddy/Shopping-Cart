using GG_shopping_cart.DTO;
using GG_shopping_cart.Entities;

namespace GG_shopping_cart.Services
{
	public interface IProductService
	{
		Task<ProductsPaginationDto> GetAllProducts(int pageNumber, int pageSize);

		Task<ProductDto?> GetProduct(string id);

		Task<ProductDto> CreateProductAsync(ProductBaseDto productDto);

		Task<ProductDto> UpdateProductAsync(ProductDto productDto);

        Task<bool> DeleteProduct(string id);
	}
}
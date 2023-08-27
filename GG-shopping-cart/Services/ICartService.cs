
using GG_shopping_cart.DTO;

namespace GG_shopping_cart.Services
{
	public interface ICartService
	{

        Task<CartDto?> GetCart(string userSession);

        Task<bool> DeleteCart(string userSession);

        Task<CartDto> CreateCartAsync(CartBaseDto cartDto);
    }
}


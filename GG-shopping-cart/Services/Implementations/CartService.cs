using Microsoft.EntityFrameworkCore;
using GG_shopping_cart.Data;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Mappers;

namespace GG_shopping_cart.Services.Implementations
{
	public class CartService: ICartService, IAutoRegister
    {
        private readonly ProductDbContext _context;

        public CartService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto?> GetCart(string userSession)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(p => p.UserSession == userSession);
            if (cart == null)
            {
                return null;
            }

            return ModelConverter.ModelToDto(cart);
        }

        public async Task<CartDto> CreateCartAsync(CartBaseDto cartDto)
        {
            var cart = ModelConverter.DtoToModel(cartDto);

            _context.Carts.Add(cart);

            await _context.SaveChangesAsync();

			return ModelConverter.ModelToDto(cart);
        }

        public async Task<bool> DeleteCart(string userSession)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(p => p.UserSession == userSession);
            if (cart == null)
            {
                return false;
            }
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}


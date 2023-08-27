using GG_shopping_cart.DTO;

namespace GG_shopping_cart.Services
{
	public interface ILineItemService
	{
        Task<List<LineItemDto>> GetCartLineItems(string cartId);

        Task<LineItemDto> UpdateLineItemAsync(LineItemDto lineItemDto);

        Task<LineItemDto> CreateLineItemAsync(LineItemBaseDto lineItemDto);

        Task<bool> DeleteLineItem(string id);
    }
}


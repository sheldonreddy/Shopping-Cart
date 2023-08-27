using GG_shopping_cart.Data;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Entities;
using GG_shopping_cart.Mappers;
using Microsoft.EntityFrameworkCore;

namespace GG_shopping_cart.Services.Implementations
{
	public class LineItemService: ILineItemService, IAutoRegister
    {
        private readonly ProductDbContext _context;

        public LineItemService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<LineItemDto>> GetCartLineItems(string cartId)
        {
            List<LineItemDto> lineItems = new List<LineItemDto>();

            var items = await _context.LineItems.Where(line => line.CartId.ToString() == cartId).ToListAsync();

            for (int i = 0; i < items.Count; i++)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == items.ElementAt(i).ProductId);

                if (product != null)
                {
                    lineItems.Add(new LineItemDto
                    {
                        Product = ModelConverter.ModelToDto(product),
                        Id = items.ElementAt(i).Id,
                        Quantity = items.ElementAt(i).Quantity,
                        CartId = items.ElementAt(i).CartId,
                    });

                }

            }
            return lineItems;
        }

        public async Task<LineItemDto> CreateLineItemAsync(LineItemBaseDto lineItemDto)
        {
            var lineItem = ModelConverter.DtoToModel(lineItemDto);

            _context.LineItems.Add(lineItem);

            await _context.SaveChangesAsync();
            

            return ModelConverter.ModelToDto(lineItem, lineItemDto.Product);
        }

        public async Task<LineItemDto> UpdateLineItemAsync(LineItemDto lineItemDto)
        {
            var lineItem = ModelConverter.DtoToModel(lineItemDto);

             _context.LineItems.Update(lineItem);
            
            await _context.SaveChangesAsync();

            lineItemDto.Id = lineItem.Id;

            return lineItemDto;
        }

        public async Task<bool> DeleteLineItem(string id)
        {
            var lineItem = await _context.LineItems.FirstOrDefaultAsync(p => p.Id.ToString() == id);
            if (lineItem == null)
            {
                return false;
            }
            _context.LineItems.Remove(lineItem);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}


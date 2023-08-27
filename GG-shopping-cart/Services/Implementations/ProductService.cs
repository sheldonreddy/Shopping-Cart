using Microsoft.EntityFrameworkCore;
using GG_shopping_cart.Data;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Mappers;

namespace GG_shopping_cart.Services.Implementations
{
    public class ProductService : IProductService, IAutoRegister
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _context.Products.Select(product =>
            ModelConverter.ModelToDto(product)).ToListAsync();

            return products;
        }

        public async Task<ProductDto?> GetProduct(string id)
        {
            var Product = await _context.Products.FirstOrDefaultAsync(p => p.Id.ToString() == id);
            if (Product == null)
            {
                return null;
            }

            return ModelConverter.ModelToDto(Product);
        }


        public async Task<ProductDto> CreateProductAsync(ProductBaseDto productDto)
        {
            var product = ModelConverter.DtoToModel(productDto);
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return ModelConverter.ModelToDto(product);
        }

        public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
        {
            var product = ModelConverter.DtoToModel(productDto);

            _context.Products.Update(product);
           
            await _context.SaveChangesAsync();

            return ModelConverter.ModelToDto(product);
        }


        public async Task<bool> DeleteProduct(string id)
        {
            var Product = await _context.Products.FirstOrDefaultAsync(p => p.Id.ToString() == id);
            if (Product == null)
            {
                return false;
            }
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();

            return true;

        }
    }
}


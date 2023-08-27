using GG_shopping_cart.Entities;
using GG_shopping_cart.DTO;

namespace GG_shopping_cart.Mappers
{
	public class ModelConverter
	{
		public static Product DtoToModel(ProductDto dto)
		{
			return new Product
			{
				Id = dto.Id,
				Title = dto.Title,
				Price = dto.Price,
				Description = dto.Description,
				CategoryId = dto.CategoryId,
				ImageUrl = dto.ImageUrl,
			};
		}

        public static Product DtoToModel(ProductBaseDto dto)
        {
            return new Product
            {
                Title = dto.Title,
                Price = dto.Price,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                ImageUrl = dto.ImageUrl,
            };
        }

        public static ProductDto ModelToDto(Product model)
		{
			return new ProductDto
			{
				Id = model.Id,
				Title = model.Title,
				Price = model.Price,
				Description = model.Description,
				CategoryId = model.CategoryId,
				ImageUrl = model.ImageUrl,
			};
		}

		public static Category DtoToModel(CategoryDto dto)
		{
			return new Category
			{
				Id = dto.Id,
				Title = dto.Title,
			};
		}

        public static Category DtoToModel(CategoryBaseDto dto)
        {
            return new Category
            {
                Title = dto.Title,
            };
        }

        public static CategoryDto ModelToDto(Category model)
        {
            return new CategoryDto
            {
                Id = model.Id,
                Title = model.Title,
            };
        }

        public static Cart DtoToModel(CartDto dto)
        {
            return new Cart
            {
                Id = dto.Id,
                UserSession = dto.UserSession,
				SubTotal = dto.SubTotal,
            };
        }

        public static Cart DtoToModel(CartBaseDto dto)
        {
            return new Cart
            {
                UserSession = dto.UserSession,
                SubTotal = dto.SubTotal,
            };
        }

        public static CartDto ModelToDto(Cart model)
        {
            return new CartDto
            {
                Id = model.Id,
                UserSession = model.UserSession,
                SubTotal = model.SubTotal,
            };
        }

        public static ApplicationUserDto ModelToDto(ApplicationUser model)
        {
            return new ApplicationUserDto
            {
                Id = model.Id,
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
            };
        }

        public static LineItem DtoToModel(LineItemDto dto)
        {
            return new LineItem
            {
                Id = dto.Id,
                ProductId = dto.Product.Id,
                Quantity = dto.Quantity,
                CartId = dto.CartId,
            };
        }

        public static LineItem DtoToModel(LineItemBaseDto dto)
        {
            return new LineItem
            {
                ProductId = dto.Product.Id,
                Quantity = dto.Quantity,
                CartId = dto.CartId,
            };
        }

        public static LineItemDto ModelToDto(LineItem model, ProductDto product)
        {
            return new LineItemDto
            {
                Id = model.Id,
                Product = product,
                Quantity = model.Quantity,
                CartId = model.CartId,
            };
        }

    }
}


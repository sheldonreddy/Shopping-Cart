
namespace GG_shopping_cart.DTO
{
	public class ProductBaseDto
	{

        public string? Title { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

    }

    public class ProductDto
    {

        public Guid? Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

    }
}




namespace GG_shopping_cart.DTO
{
    public class LineItemBaseDto
    {

        public Guid? CartId { get; set; }

        public ProductDto? Product { get; set; }

        public int Quantity { get; set; }

    }

    public class LineItemDto
    {

        public Guid? Id { get; set; }

        public Guid? CartId { get; set; }

        public ProductDto? Product { get; set; }

        public int Quantity { get; set; }

    }
}


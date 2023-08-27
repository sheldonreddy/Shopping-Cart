
namespace GG_shopping_cart.DTO
{
	public class CartBaseDto
	{
        public string? UserSession { get; set; }

        public Decimal SubTotal { get; set; }
    }

    public class CartDto
    {
        public Guid? Id { get; set; }

        public string? UserSession { get; set; }

        public Decimal SubTotal { get; set; }
    }
}


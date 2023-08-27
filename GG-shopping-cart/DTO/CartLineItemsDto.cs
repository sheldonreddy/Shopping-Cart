namespace GG_shopping_cart.DTO
{
	public class CartLineItemsDto
	{
		public Guid? Id { get; set; }
		public Decimal SubTotal { get; set; }
		public List<LineItemDto> Items { get; set; } = new List<LineItemDto>();

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (CartLineItemsDto)obj;

            return Id == other.Id && SubTotal == other.SubTotal;
        }
    }
}


using System;
namespace GG_shopping_cart.DTO
{
	public class ProductsPaginationDto
    {
		public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<ProductDto>? Products { get; set; }
	}
}


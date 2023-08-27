using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GG_shopping_cart.Entities
{
	public class LineItem
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        [Required]
		public Guid? CartId { get; set; }

		[Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
		
		[Required]
		public Guid? ProductId { get; set; }

        [Required]
        public Product? Product { get; set; }

        [Required]
        public Cart? Cart { get; set; }
    }
}


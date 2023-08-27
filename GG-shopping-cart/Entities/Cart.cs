using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GG_shopping_cart.Entities
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        [Required]
        public string? UserSession { get; set; }

        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Subtotal must no be less than 0")]
        public Decimal SubTotal { get; set; }
    }
}


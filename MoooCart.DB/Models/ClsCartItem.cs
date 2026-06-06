using System.ComponentModel.DataAnnotations;

namespace MoooCart.DB.Models
{
    public class ClsCartItem
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.DTOs
{
    public class DtoProductHistory
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }

    public class DtoCart
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class DtoCheckout
    {
        //[Required]
        //public Guid PaymentMethodId { get; set; }
        [Required]
        public IEnumerable<DtoCart> Carts { get; set; }
    }

    public class DtoCartItemRequest
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
    }

    public class DtoCartItemResponse
    {
        public Guid Id { get; set; } 
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImageUrl { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }

    public class DtoCartResponse
    {
        public IEnumerable<DtoCartItemResponse> Items { get; set; } = [];
        public decimal TotalCartPrice => Items.Sum(x => x.TotalPrice);
    }

}

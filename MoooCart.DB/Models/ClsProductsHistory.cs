using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.DB.Models
{
    public class ClsProductsHistory
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasedPrice { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.DB.Models
{
    public class ClsOrder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public ICollection<ClsOrderItem> OrderItems { get; set; } = new List<ClsOrderItem>();
    }
}

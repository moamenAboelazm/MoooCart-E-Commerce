using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.DTOs
{
    public class DtoOrderHistory
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<DtoOrderItemHistory> Items { get; set; } = new List<DtoOrderItemHistory>();
    }

    public class DtoOrderItemHistory
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

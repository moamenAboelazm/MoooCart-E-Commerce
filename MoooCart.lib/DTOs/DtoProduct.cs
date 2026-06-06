using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MoooCart.lib.DTOs
{
    public class DtoProduct
    {
        [Required]
        public string ProductSerial { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public Guid? CategoryId { get; set; }
        public string? ImageUrl { get; set; } = "https://kints.co.in/twenty-nineteen/img/defaults/product-default.png";
        public int Quantity { get; set; } = 0;
    }
    public class DtoUpdateProduct : DtoProduct
    {
        public string ID { get; set; }
    }
    public class DtoGetProducts : DtoProduct
    {
        public string ID { get; set; }
        public string CategoryName { get; set; }
    }
}




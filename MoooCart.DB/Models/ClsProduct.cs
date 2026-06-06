using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.DB.Models
{
    public class ClsProduct
    {
        [Key]
        public Guid ID { get; set; }
        //---------------------------------------
        [Required(ErrorMessage ="Product Serial can not be Empty")]
        public string ProductSerial { get; set; }
        //---------------------------------------
        [Required(ErrorMessage = "Product Name can not be Empty")]
        [MaxLength(30, ErrorMessage = "Product Name Max Length is 30 Characters")]
        public string Name { get; set; }
        //---------------------------------------
        [MaxLength(200, ErrorMessage = "Product Description Max Length is 200 Characters")]
        public string? Description { get; set; }
        //---------------------------------------
        [Required]
        public decimal Price { get; set; }
        //---------------------------------------
        public int Quantity { get; set; } = 0;
        //---------------------------------------
        public string? ImageUrl { get; set; } = "https://kints.co.in/twenty-nineteen/img/defaults/product-default.png";
        //---------------------------------------
        public decimal AvgRating { get; set; } = 0;
        //---------------------------------------
        public Guid CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public ClsCategory Category { get; set; }
        //---------------------------------------

    }
}

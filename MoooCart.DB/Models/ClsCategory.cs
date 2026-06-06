using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.DB.Models
{
    public class ClsCategory
    {
        public Guid ID { get; set; }
        //------------------------------------
        [Required(ErrorMessage = "Category Name Can not be Empty")]
        [MaxLength(25, ErrorMessage = "Category Name Max Length is 30 Characters")]
        public string Name { get; set; }
        //------------------------------------
        public ICollection<ClsProduct>? Products { get; set; }
    }
}

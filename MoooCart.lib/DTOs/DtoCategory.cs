using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.DTOs
{
    public class DtoCategory
    {
        public string Name { get; set; }
    }
    public class DtoUpdateCategory : DtoCategory
    {
        public string ID { get; set; }
    }

    public class DtoGetCategory : DtoCategory
    {
        public string ID { get; set; }
        public IEnumerable<DtoProduct> Products { get; set; }
    }
}

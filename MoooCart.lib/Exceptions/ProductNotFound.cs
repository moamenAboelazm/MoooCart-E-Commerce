using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.Exceptions
{
    public class ProductNotFound(String msg) : Exception(msg)
    {
    }
}

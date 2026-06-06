using MoooCart.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.DB.Base
{
    public interface ICart
    {
        Task<int> SaveCheckoutHistory(IEnumerable<ClsProductsHistory> checkouts);

    }
}

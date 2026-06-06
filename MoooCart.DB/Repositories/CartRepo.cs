using MoooCart.DB.Base;
using MoooCart.DB.Contexts;
using MoooCart.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.DB.Repositories
{
    public class CartRepo(AppDbContext _context) : ICart
    {
        public async Task<int> SaveCheckoutHistory(IEnumerable<ClsProductsHistory> checkouts)
        {
            await _context.ProductsHistories.AddRangeAsync(checkouts);
            return await _context.SaveChangesAsync();
        }
    }
}

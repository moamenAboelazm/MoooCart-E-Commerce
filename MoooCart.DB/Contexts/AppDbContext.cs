using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoooCart.DB.Models;
using MoooCart.id.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.DB.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ClsProduct> Products { get; set; }
        public DbSet<ClsCategory> Categories { get; set; }   
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ClsProductsHistory> ProductsHistories { get; set; }
        public DbSet<ClsCartItem> CartItems { get; set; }
        public DbSet<ClsOrder> Orders { get; set; }
        public DbSet<ClsOrderItem> OrderItems { get; set; }

    }
}

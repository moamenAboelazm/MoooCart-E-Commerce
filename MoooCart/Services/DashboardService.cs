using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MoooCart.DB.Contexts;
using MoooCart.id.Identity;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;

namespace MoooCart.Services
{
    public class DashboardService(AppDbContext _context, UserManager<AppUser> _userManager, IMemoryCache _cache) : IDashboardService
    {
        public async Task<DtoDashboardStatistics> GetStatisticsAsync()
        {
            const string cacheKey = "DashboardStatsCache";

            if (_cache.TryGetValue(cacheKey, out DtoDashboardStatistics? cachedStats))
            {
                return cachedStats!;
            }
            var totalUsers = await _userManager.Users.CountAsync();
            var totalProducts = await _context.Products.CountAsync();
            var totalProductsSold = await _context.ProductsHistories.SumAsync(x => x.Quantity);

            var totalRevenue = await _context.ProductsHistories
                .Join(_context.Products,
                      history => history.ProductId,
                      product => product.ID,
                      (history, product) => new { history.Quantity, product.Price })
                .SumAsync(x => x.Quantity * x.Price);

            var stats = new DtoDashboardStatistics
            {
                TotalUsers = totalUsers,
                TotalProducts = totalProducts,
                TotalProductsSold = totalProductsSold,
                TotalRevenue = totalRevenue
            };

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, stats, cacheOptions);

            return stats;
        }
    }
}
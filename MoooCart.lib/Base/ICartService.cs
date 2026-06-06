using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.Base
{
    public interface ICartService
    {
        Task<DtoResponse> SaveCheckoutHistory(IEnumerable<DtoCart> checkouts, string userId);
        Task<DtoResponse> DoCheckout(string userId);
        Task<DtoCartResponse> GetCartAsync(string userId);
        Task<DtoResponse> AddItemToCartAsync(DtoCartItemRequest request, string userId);
        Task<DtoResponse> UpdateItemQuantityAsync(Guid cartItemId, int quantity, string userId);
        Task<List<DtoOrderHistory>> GetUserOrderHistory(string userId);
        Task<List<DtoOrderHistory>> GetAllOrdersAsync();
        Task<DtoResponse> DeleteItemFromCartAsync(Guid cartItemId, string userId);

    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoooCart.DB.Base;
using MoooCart.DB.Contexts;
using MoooCart.DB.Models;
using MoooCart.lib.Base;
using MoooCart.lib.DTOs;

namespace MoooCart.Services
{
    public class CartServices(AppDbContext _context, IMapper _mapper, IGenericRepository<ClsProduct> _productRepo) : ICartService
    {
        public async Task<DtoCartResponse> GetCartAsync(string userId)
        {
            var userGuid = Guid.Parse(userId);

            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userGuid)
                .Join(_context.Products,
                      cart => cart.ProductId,
                      product => product.ID,
                      (cart, product) => new DtoCartItemResponse
                      {
                          Id = cart.ID,
                          ProductId = cart.ProductId,
                          ProductName = product.Name,
                          ProductImageUrl = product.ImageUrl,
                          UnitPrice = product.Price,
                          Quantity = cart.Quantity
                      }).ToListAsync();

            return new DtoCartResponse { Items = cartItems };
        }

        public async Task<DtoResponse> AddItemToCartAsync(DtoCartItemRequest request, string userId)
        {
            var userGuid = Guid.Parse(userId);

            var product = await _productRepo.GetByIDAsync(request.ProductId);
            if (product == null) return new DtoResponse(false, "Product not found.");
            if (product.Quantity < request.Quantity) return new DtoResponse(false, "Not enough stock available.");

            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userGuid && c.ProductId == request.ProductId);

            if (existingCartItem != null)
            {
                if (product.Quantity < (existingCartItem.Quantity + request.Quantity))
                    return new DtoResponse(false, "Cannot add more than available stock.");

                existingCartItem.Quantity += request.Quantity;
                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                var newCartItem = new ClsCartItem
                {
                    ID = Guid.NewGuid(),
                    UserId = userGuid,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                await _context.CartItems.AddAsync(newCartItem);
            }

            await _context.SaveChangesAsync();
            return new DtoResponse(true, "Product added to cart successfully.");
        }

        public async Task<DtoResponse> UpdateItemQuantityAsync(Guid cartItemId, int quantity, string userId)
        {
            var userGuid = Guid.Parse(userId);

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ID == cartItemId && c.UserId == userGuid);
            if (cartItem == null) return new DtoResponse(false, "Item not found in your cart.");

            var product = await _productRepo.GetByIDAsync(cartItem.ProductId);
            if (product == null) return new DtoResponse(false, "Product not found.");
            if (product.Quantity < quantity) return new DtoResponse(false, $"Only {product.Quantity} items left in stock.");

            cartItem.Quantity = quantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return new DtoResponse(true, "Cart item quantity updated successfully.");
        }

        public async Task<DtoResponse> DeleteItemFromCartAsync(Guid cartItemId, string userId)
        {
            var userGuid = Guid.Parse(userId);

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ID == cartItemId && c.UserId == userGuid);
            if (cartItem == null) return new DtoResponse(false, "Item not found in your cart.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return new DtoResponse(true, "Item removed from cart successfully.");
        }

        public async Task<DtoResponse> DoCheckout(string userId)
        {
            var userGuid = Guid.Parse(userId);

            var cartItems = await _context.CartItems.Where(c => c.UserId == userGuid).ToListAsync();
            if (!cartItems.Any()) return new DtoResponse(false, "Cart is empty.");

            var cartProductIds = cartItems.Select(c => c.ProductId).ToList();
            var products = await _productRepo.FindAllAsync(p => cartProductIds.Contains(p.ID));

            foreach (var item in cartItems)
            {
                var dbProduct = products.FirstOrDefault(p => p.ID == item.ProductId);
                if (dbProduct == null) return new DtoResponse(false, "One of the products in your cart no longer exists.");
                if (dbProduct.Quantity < item.Quantity) return new DtoResponse(false, $"Not enough stock for product: {dbProduct.Name}");
            }

            decimal totalAmount = 0;
            var newOrder = new ClsOrder
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow
            };

            foreach (var item in cartItems)
            {
                var dbProduct = products.First(p => p.ID == item.ProductId);

                dbProduct.Quantity -= item.Quantity;
                await _productRepo.UpdateAsync(dbProduct);

                newOrder.OrderItems.Add(new ClsOrderItem
                {
                    ProductId = dbProduct.ID,
                    Quantity = item.Quantity,
                    UnitPrice = dbProduct.Price
                });

                totalAmount += item.Quantity * dbProduct.Price;
            }

            newOrder.TotalAmount = totalAmount;

            await _context.Orders.AddAsync(newOrder);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return new DtoResponse(true, $"Checkout completed successfully! Total amount to pay on delivery: {totalAmount:C}");
        }

        public async Task<List<DtoOrderHistory>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();

            var products = await _productRepo.GetAllAsync();

            var history = orders.Select(o => new DtoOrderHistory
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new DtoOrderItemHistory
                {
                    ProductName = products.FirstOrDefault(p => p.ID == oi.ProductId)?.Name ?? "Unknown Product",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();

            return history;
        }

        public async Task<List<DtoOrderHistory>> GetUserOrderHistory(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ToListAsync();

            var orderProductIds = orders.SelectMany(o => o.OrderItems.Select(oi => oi.ProductId)).Distinct().ToList();

            var products = await _productRepo.FindAllAsync(p => orderProductIds.Contains(p.ID));

            var history = orders.Select(o => new DtoOrderHistory
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new DtoOrderItemHistory
                {
                    ProductName = products.FirstOrDefault(p => p.ID == oi.ProductId)?.Name ?? "Unknown Product",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();

            return history;
        }

        public async Task<DtoResponse> SaveCheckoutHistory(IEnumerable<DtoCart> checkouts, string userId)
        {
            var mappedData = _mapper.Map<List<ClsProductsHistory>>(checkouts);
            foreach (var item in mappedData)
            {
                item.UserId = Guid.Parse(userId);
            }
            await _context.ProductsHistories.AddRangeAsync(mappedData);
            await _context.SaveChangesAsync();
            return new DtoResponse(true, "Checkout saved successfully");
        }
    }
}
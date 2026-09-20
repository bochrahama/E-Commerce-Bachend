using EcommerceBackend.Data;
using System.Threading.Tasks;
using EcommerceBackend.DTOs;
using Microsoft.Extensions.Caching.Memory;
using EcommerceBackend.Entities;
using Microsoft.EntityFrameworkCore;


namespace EcommerceBackend.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        public CartService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        // The purpose of this method is to retrieve the cart for a specific user.
        // If the cart does not exist, it creates a new cart for that user.
        //it's privat because it's only used internally within
        //the CartService class and not intended to be accessed from outside.
        private async Task<CartDto> GetorCreatCartAsync(string userId)
        {
            // If not in cache, retrieve from database (simulated here)
            var cart = await _context.Carts
            .Include(c => c.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = userId,
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return MapToDto(cart);
        }
        private static CartDto MapToDto(Cart cart)
        {
            return new CartDto
            {
                CartId = cart.CartId,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    CartItemId = i.ProductId,
                    ProductName = i.Product!.ProductName,
                    Price = i.Product.ProductPrice,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
        // The purpose of this method is to retrieve the cart for a specific user.

        public Task<CartDto> GetCartAsync(string userId)
        {
            return GetorCreatCartAsync(userId);
        }

        //the porpes is to add an item to the cart, if the cart does not exist for the user,
        //it creates a new cart. If the item already exists in the cart, it updates the quantity.
        //If the item does not exist, it adds a new item to the cart.
        public async Task<CartDto> AddItemToCartAsync(string userId, CartItemDto cartItemDto)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = userId,
                };
                _context.Carts.Add(cart);
            }
            // Check if the item already exists in the cart
            //FirstOrDefault returns the first element of a sequence, or a default value if no element is found.
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == cartItemDto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;
            }
            else // If the item does not exist, add a new item to the cart
            {
                var product = await _context.Products.FindAsync(cartItemDto.ProductId);
                if (product is null)
                {
                    // If the product doesn't exist, throw an exception or handle it as needed
                    throw new Exception("Product not found");
                }
                var newItem = new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = cartItemDto.Quantity,
                    Cart = cart
                };
                cart.Items.Add(newItem);
            }
            await _context.SaveChangesAsync();
            // After saving changes, return the updated cart
            return await GetCartAsync(userId);
        }
        // The purpose of this method is to update the quantity of a specific item in the user's cart.
        public async Task<bool> UpdateQuantityAsync(string userId, int CartItemId, int Quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
            {
                return false; // Cart not found
            }
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == CartItemId);
            if (existingItem is null)
            {
                return false; // Cart item not found
            }
            existingItem.Quantity = Quantity;
            await _context.SaveChangesAsync();
            return true; // Successfully updated
        }
        public async Task<bool> RemoveItemFromCartAsync(string userId, int productId)
       => await UpdateQuantityAsync(userId, productId, 0);

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _context.Carts
         .Include(c => c.Items)
         .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart is null) return;
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
        }
        //in update the bast way is return false or true



        /*public async Task<CartDto> UpdateQuantityAsync(string userId, int CartItemId, int Quantity)
        {
            var cart = await GetorCreatCartAsync(userId);
         
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == CartItemId);
            if (existingItem is null)
            {
                throw new Exception("Cart item not found");
            }
            existingItem.Quantity = Quantity;
            await _context.SaveChangesAsync();
            return await GetCartAsync(userId);
        */
    }

}

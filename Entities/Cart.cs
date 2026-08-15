using System;
using System.Collections.Generic;

namespace EcommerceBackend.Entities
{
    public class Cart
    {
        public int CartId { get; private set; }
        // Identifier for the user who owns the cart
        public Guid UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;


        public Cart(){}
    }
    
}
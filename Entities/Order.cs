using System;
using System.Collections.Generic;

namespace EcommerceBackend.Entities
{

    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string OrderNumber { get; set; } = Guid.NewGuid().ToString();
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalAmount => CalculateTotal();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Order() { }

        private decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (var item in Items)
            {
                total += item.TotalPrice;
            }
            return total;
        }
    }
}

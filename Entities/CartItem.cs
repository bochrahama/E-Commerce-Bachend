using System;

namespace EcommerceBackend.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        //DateTimeOffset is used to store the date and time along with the offset from UTC, which is useful for applications that need to handle time zones.
        //ex 2024-06-15T14:30:00+02:00 represents June 15, 2024, at 2:30 PM in a time zone that is 2 hours ahead of UTC.
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

     public CartItem() { }
    }

}
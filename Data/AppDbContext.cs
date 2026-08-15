using EcommerceBackend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  

namespace EcommerceBackend.Data
{
    public partial class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        protected override void OnModelCreating(ModelBuilder Builder)
        {
            base.OnModelCreating(Builder);
            // Configure the relationship between Cart and CartItem
            Builder.Entity<Product>()
                .Property(p => p.ProductPrice)
                .HasPrecision(18, 2); // Set precision and scale for decimal type

            Builder.Entity<Order>() 
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2); // Set precision and scale for decimal type

            Builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2); // Set precision and scale for decimal type

            Builder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: specify delete behavior

        }
    }
}

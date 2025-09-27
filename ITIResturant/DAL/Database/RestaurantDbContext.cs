using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.DAL.Entities;

namespace Restaurant.DAL.Database
{
    public class RestaurantDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Table> Tables { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<PromoCode> PromoCodes { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<EmailNotification> EmailNotifications { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<AppUser> AppUsers { get; set; } = null!;
        public DbSet<ShopingCartItem> ShopingCartItems { get; set; } = null!;
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order -> Delivery
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Delivery)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DeliveryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Order -> Customer
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders) // Explicitly specify the Orders navigation property
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message -> Order
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Order)
                .WithMany(o => o.Messages)
                .HasForeignKey(m => m.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order decimal configs
            modelBuilder.Entity<Order>()
                .Property(o => o.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            // OrderItem decimal configs
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Quantity)
                .HasColumnType("decimal(18,2)");

            // Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // PromoCode
            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.DiscountValue)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.MinimumOrderAmount)
                .HasColumnType("decimal(18,2)");

            // ShoppingCartItem
            modelBuilder.Entity<ShopingCartItem>()
                .Property(sci => sci.Quantity)
                .HasColumnType("decimal(18,2)");

            // Payment enums as string
            modelBuilder.Entity<Payment>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .Property(p => p.PayMethod)
                .HasConversion<string>();

            // Order status enum as string
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            // Message -> Sender
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            // Message -> Receiver
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Reciver)
                .WithMany(u => u.ReciveMessages)
                .HasForeignKey(m => m.ReciverID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
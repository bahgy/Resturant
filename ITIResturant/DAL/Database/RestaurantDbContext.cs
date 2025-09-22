

using Microsoft.EntityFrameworkCore;

namespace Restaurant.DAL.Database
{
    public class RestaurantDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Table> Tables { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<PromoCode> PromoCodes { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<EmailNotification> EmailNotifications { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<AppUser> AppUsers { get; set; } = null!;
        public DbSet<ShopingCartItem> ShopingCartItems { get; set; } = null!;
        public DbSet<Table> Table { get; set; } = null!;
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments  { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Order)
                .WithMany(o => o.Feedbacks)
                .HasForeignKey(f => f.OrderId)
                .OnDelete(DeleteBehavior.NoAction);  // prevent multiple cascade paths

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Customer)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);  // also safe option
            modelBuilder.Entity<Order>()
        .Property(o => o.DiscountAmount)
        .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.DiscountValue)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.MinimumOrderAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ShopingCartItem>()
                .Property(sci => sci.Quantity)
                .HasColumnType("decimal(18,2)");
           modelBuilder
                .Entity<Payment>()
                .Property(p => p.Status)
                .HasConversion<string>();

                    modelBuilder
                        .Entity<Payment>()
                        .Property(p => p.Method)
                .HasConversion<string>();
        }

    }
}

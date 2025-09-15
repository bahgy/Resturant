using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataBase
{
    public class ResturantDbContext:DbContext
    { public DbSet<User> Users { get; set; }    
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer>Customers { get; set; }
        public DbSet<Cart>Carts { get; set; }
        public   DbSet<Category> Categories { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<ShopingCartItem> ShopingCartItems { get; set; }
        public DbSet<Table> Tables { get; set; }

        
        // public DbSet<PromoCode>promoCodes { get; set; }
        public ResturantDbContext(DbContextOptions<ResturantDbContext> options) : base(options) { }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Order>()
        //        .HasOne(o => o.Feedback)
        //        .WithOne(f => f.Order)
        //        .HasForeignKey<Feedback>(f => f.OrderId); // هنا بنحدد الـ dependent
        //                                                  // مثال تعطيل Cascade Delete بين Feedback و User
        //    modelBuilder.Entity<Feedback>()
        //        .HasOne(f => f.Customer)
        //        .WithMany(u => u.Feedbacks)
        //        .HasForeignKey(f => f.CustomerId)
        //        .OnDelete(DeleteBehavior.Restrict); // NO ACTION
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Feedback ↔ Customer
            //modelBuilder.Entity<Feedback>()
            //    .HasOne(f => f.Customer)
            //    .WithMany(c => c.Feedbacks)
            //    .HasForeignKey(f => f.CustomerId)
            //    .OnDelete(DeleteBehavior.Cascade);
            // Feedback ↔ Customer/User
            //modelBuilder.Entity<Feedback>()
            //    .HasOne(f => f.Customer)         // Navigation property
            //    .WithMany(u => u.Feedbacks)      // Navigation property
            //    .HasForeignKey(f => f.CustomerId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// Feedback ↔ Order
            //modelBuilder.Entity<Feedback>()
            //    .HasOne(f => f.Order)
            //    .WithMany(o => o.Feedback)
            //    .HasForeignKey(f => f.OrderId)
            //    .OnDelete(DeleteBehavior.Cascade);
            // Feedback ↔ Customer/User
            modelBuilder.Entity<Feedback>()
                 .HasOne(f => f.Customer)
                 .WithMany(u => u.Feedbacks)
                 .HasForeignKey(f => f.CustomerId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Order)
                .WithMany(o => o.Feedback)
                .HasForeignKey(f => f.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            // OrderItem ↔ Product
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem ↔ Order
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ShoppingCartItem ↔ Cart
            modelBuilder.Entity<ShopingCartItem>()
                .HasOne(sci => sci.Cart)
                .WithMany(c => c.ShopingCartItem)
                .HasForeignKey(sci => sci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // ShoppingCartItem ↔ Product
            modelBuilder.Entity<ShopingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany(p => p.shopingCartItems)
                .HasForeignKey(sci => sci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category ↔ Product
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cart ↔ Customer (One-to-One)
            
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithOne(cus => cus.Cart)
                .HasForeignKey<Cart>(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // بدل Cascade

        }





    }
}

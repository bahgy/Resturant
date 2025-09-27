
namespace Restaurant.DAL.Entities
{
    public class Customer : AppUser
    {
        public string? DefaultDeliveryAddress { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<EmailNotification> EmailNotifications { get; set; } = new List<EmailNotification>();
        public string? ConfirmationToken { get; set; }
    }
}
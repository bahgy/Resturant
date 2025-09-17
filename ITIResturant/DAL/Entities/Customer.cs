
    namespace Restaurant.DAL.Entities
    {
        public class Customer : AppUser
        {
            public bool EmailVerified { get; set; }
            public string? DefaultDeliveryAddress { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Order> Orders { get; set; }

        public ICollection<EmailNotification> EmailNotifications { get; set; }

        public string? ConfirmationToken { get; set; }  // temporary token
    }
    }


namespace Restaurant.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime TimeRequst { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime EstimatDelivryTime { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentState { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!; // Navigation property
        public string DelivryAddress { get; set; } = null!;
        public int? PromoCodeId { get; set; }
        public PromoCode? PromoCode { get; set; }
        public int? DeliveryId { get; set; }
        public Delivery? Delivery { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}

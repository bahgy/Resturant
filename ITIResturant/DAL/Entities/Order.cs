
namespace Restaurant.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime TimeRequst { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string status { get; set; }
        public DateTime EstimatDelivryTime { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus paymentSTate { get; set; }
        public int customerId { get; set; }
        public Customer Customer { get; set; }

        public string DelivryAddress { get; set; }
        public int? PromoCodeId { get; set; }
        public PromoCode PromoCode { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }
}

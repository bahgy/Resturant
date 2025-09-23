

namespace Restaurant.BLL.ModelVMOrder
{
    public class UpdateOrderVM
    {
        [Required]
        public int Id { get; set; }

        [StringLength(500)]
        public string DelivryAddress { get; set; }

        public OrderStatus Status { get; set; }
        public PaymentStatus? PaymentState { get; set; }   // enum instead of string
        public PaymentMethod? PaymentMethod { get; set; }  // enum instead of string
    }
}

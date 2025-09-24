

namespace Restaurant.BLL.ModelVMOrder
{
    public class CreateOrderVM
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(500)]
        public string DelivryAddress { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public string PromoCode { get; set; }

        [Required]
        public List<CreateOrderItemVM> OrderItems { get; set; }
    }
}

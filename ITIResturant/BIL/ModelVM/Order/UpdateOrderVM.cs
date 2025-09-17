

namespace Restaurant.BLL.ModelVMOrder
{
    public class UpdateOrderVM
    {
        [Required]
        public int Id { get; set; }

        [StringLength(500)]
        public string DelivryAddress { get; set; }

        public string Status { get; set; }
        public string PaymentState { get; set; }
        public string PaymentMethod { get; set; }
    }
}

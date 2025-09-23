

namespace Restaurant.BLL.ModelVMOrder
{
    public class OrderStatusUpdateVM
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public DateTime? EstimatDelivryTime { get; set; }
    }
}

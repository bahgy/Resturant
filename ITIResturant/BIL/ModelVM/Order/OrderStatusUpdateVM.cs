

namespace Restaurant.BLL.ModelVMOrder
{
    public class OrderStatusUpdateVM
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime? EstimatDelivryTime { get; set; }
    }
}

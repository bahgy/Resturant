using Restaurant.BLL.ModelVMOrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.ModelVMOrder
{
    public class OrderVM
    {
        public int Id { get; set; }
        public DateTime TimeRequst { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount => TotalAmount - DiscountAmount;
        public string Status { get; set; }
        public DateTime EstimatDelivryTime { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentState { get; set; }
        public int CustomerId { get; set; }

        // Updated customer properties
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public string DelivryAddress { get; set; }

        public int? PromoCodeId { get; set; }
        public string PromoCode { get; set; }
        public string PromoCodeDescription { get; set; }

        public int TotalItems { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }
    }
}

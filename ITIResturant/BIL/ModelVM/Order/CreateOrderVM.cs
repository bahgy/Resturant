using BIL.ModelVM.OrderItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.ModelVM.Order
{
    public class CreateOrderVM
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(500)]
        public string DelivryAddress { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public string PromoCode { get; set; }

        [Required]
        public List<CreateOrderItemVM> OrderItems { get; set; }
    }
}

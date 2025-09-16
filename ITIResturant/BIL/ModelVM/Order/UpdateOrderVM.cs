using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.ModelVM.Order
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

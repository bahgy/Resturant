using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

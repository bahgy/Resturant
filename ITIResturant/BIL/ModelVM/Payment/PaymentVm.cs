using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.ModelVM.Payment
{
   public class PaymentVm
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }  // مثلاً: Card, Cash, Paypal
        public PaymentStatus Status { get; set; }         // Pending, Completed, Failed
        public string TransactionReference { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int OrderNumber { get; set; }
    }
}

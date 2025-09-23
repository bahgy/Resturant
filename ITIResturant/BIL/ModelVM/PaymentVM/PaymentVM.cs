using System;
using Restaurant.DAL.Enum;

namespace Resturant.BLL.ModelVM.PaymentVM
{
    public class PaymentVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }  // instead of "Method"
        public PaymentStatus Status { get; set; }
        public string TransactionReference { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}

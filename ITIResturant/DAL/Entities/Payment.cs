using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.DAL.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }   // PK
        public int OrderID { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public PaymentMethod PayMethod { get; set; }   // Cash, Card, Paypal

        [Precision(18, 2)]
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }   // Pending, Paid, Failed, Refunded

        public string TransactionReference { get; set; }

        public bool IsSuccessful { get; set; }

        // Navigation Property
        public Order Order { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }   // PK
        public int OrderID { get; set; }     
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }   // Cash, Card, Online
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }   // Pending, Paid, Failed, Refunded
        public string TransactionReference { get; set; }
        public bool IsSuccsessful { get; set; }

        // Navigation Property
        public Order Order { get; set; }

    }
}

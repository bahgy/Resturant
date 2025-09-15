using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Feedback
    {
        public Feedback()
        {
        }

        public Feedback(int id, string comment, int rating, DateTime submittedDate, int orderId, Order order, int customerId, Customer customer)
        {
            Id = id;
            Comment = comment;
            Rating = rating;
            SubmittedDate = submittedDate;
            OrderId = orderId;
            Order = order;
            CustomerId = customerId;
            Customer = customer;
        }

        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime SubmittedDate { get;  set; }

        // Order relationship
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Customer relationship
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Customer:User
    { public bool EmialVerified { get;set;}
        public string defaultDelivryAddress { get;set;}
        public  Cart Cart { get;set;}
        public List<Order> orders { get;set;}
        public List<Booking> bookings { get; set; }
        public List<Feedback> Feedbacks { get; set; }

    }
}

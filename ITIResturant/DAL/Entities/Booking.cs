using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int numberOfGuests {  get; set; }
        public string status { get; set; }
        public int TableNumber { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}

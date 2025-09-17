using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Entities
{
    public class Booking
    {
               public Booking(DateTime bookingDate, DateTime startTime, DateTime endTime, int numberOfGuests, string status, string specialRequests, int customerId , int tableId)
        {
            BookingDate = bookingDate;
            StartTime = startTime;
            EndTime = endTime;
            NumberOfGuests = numberOfGuests;
            Status = status;
            SpecialRequests = specialRequests;
            CustomerId = customerId;
  
            CreatedOn = DateTime.Now;
            TableId = tableId;

        }

        public int Id { get; private set; }
        public DateTime BookingDate { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int NumberOfGuests { get; private set; }
        public string Status { get; private set; }
        public string SpecialRequests { get; private set; }
        public int CustomerId { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public bool? IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }



        public int TableId { get; private set; }
        public Table Table { get; private set; }


        public bool Update(DateTime bookingDate, DateTime startTime, DateTime endTime, int numberOfGuests, string status, string specialRequests, int customerId, int tableId)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return false;
            }
            BookingDate = bookingDate;
            StartTime = startTime;
            EndTime = endTime;
            NumberOfGuests = numberOfGuests;
            Status = status;
            SpecialRequests = specialRequests;
            CustomerId = customerId;
            ModifiedOn = DateTime.Now;
            TableId =  tableId;
            return true;
        }


        public bool Toggelstatus(int Deletedbooking)
        {
            if (Deletedbooking<0)
            { return false; }
            else
            {
                IsDeleted = !IsDeleted;
                DeletedOn = DateTime.Now;
                return true;
            }
        }

    }
}

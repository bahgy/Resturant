
namespace Restaurant.BLL.Model_VM.Booking
{
    public class GetAllBookingVM
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; }
        public string SpecialRequests { get; set; }
        public int CustomerId { get; set; }
        public int TableId { get; set; } // صححت من TableNumber إلى TableId
    }
}

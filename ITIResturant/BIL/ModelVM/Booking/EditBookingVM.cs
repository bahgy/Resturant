
namespace Restaurant.BLL.Model_VM.Booking
{
    public class EditBookingVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Booking Date is required")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Start Time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required")]
        public DateTime EndTime { get; set; }

        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = "Pending";
        public string SpecialRequests { get; set; }
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please select a table")]
        public int? TableId { get; set; }
    }
}

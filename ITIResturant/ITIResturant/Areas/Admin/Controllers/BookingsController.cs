using Microsoft.AspNetCore.Mvc;
using Restaurant.BLL.Service.Abstraction;

namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            var (success, message, bookings) = _bookingService.GetAll();

            if (!success)
            {
                TempData["Error"] = message;
                bookings = new List<Restaurant.BLL.Model_VM.Booking.GetAllBookingVM>();
            }

            return View(bookings);
        }
    }
}

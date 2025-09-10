using BIL.Model_VM.Booking;
using BIL.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace RestoPL.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ITableService _tableService;

        public BookingController(IBookingService bookingService, ITableService tableService)
        {
            _bookingService = bookingService;
            _tableService = tableService;
        }

        //=========================================================
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Tables = new SelectList(_tableService.GetAllActiveTables(), "Id", "TableNumber");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBookingVM bookingVM)
        {
            if (ModelState.IsValid)
            {
                var result = _bookingService.Create(bookingVM);
                if (!result.Item1)
                    return RedirectToAction("GetAll");

                ViewBag.error = result.Item2;
            }

            // إعادة تحميل الطاولات في حالة الخطأ
            ViewBag.Tables = new SelectList(_tableService.GetAllActiveTables(), "Id", "TableNumber");
            return View(bookingVM);
        }

        //=========================================================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _bookingService.GetById(id);
            if (result.Item1) // فيه مشكلة
            {
                ViewBag.error = result.Item2;
                return RedirectToAction("GetAll");
            }

            // إعادة تحميل الطاولات للـ dropdown
            ViewBag.Tables = new SelectList(_tableService.GetAllActiveTables(), "Id", "TableNumber");

            return View(result.Item3); // EditBookingVM
        }

        [HttpPost]
        public IActionResult Edit(int id, EditBookingVM editBookingVM)
        {
            if (ModelState.IsValid)
            {
                var result = _bookingService.Edit(id, editBookingVM);
                if (!result.Item1)
                    return RedirectToAction("GetAll");

                ViewBag.error = result.Item2;
            }

            // إعادة تحميل الطاولات في حالة الخطأ
            ViewBag.Tables = new SelectList(_tableService.GetAllActiveTables(), "Id", "TableNumber");

            return View(editBookingVM);
        }

        //=========================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bookingService.GetAll();
            if (result.Item1) // فيه مشكلة
            {
                ViewBag.error = result.Item2;
            }

            return View(result.Item3); // الليست
        }

        //=========================================================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _bookingService.Delete(id);
            if (!result)
                ViewBag.error = "Failed to delete booking.";

            return RedirectToAction("GetAll");
        }
    }
}

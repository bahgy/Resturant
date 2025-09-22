

namespace RestoPL.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(ValidateUserExistsFilter))]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ITableService _tableService;
        private readonly UserManager<AppUser> _userManager;

        public BookingController(
            IBookingService bookingService,
            ITableService tableService,
            UserManager<AppUser> userManager)
        {
            _bookingService = bookingService;
            _tableService = tableService;
            _userManager = userManager;
        }

        //=========================================================
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Tables = new SelectList(_tableService.GetAllActiveTables(), "Id", "TableNumber");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingVM bookingVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ViewBag.error = "You must be logged in to create a booking.";
                    return View(bookingVM);
                }

                bookingVM.CustomerId = user.Id;

                var result = _bookingService.Create(bookingVM);
                if (result.Item1)
                {
                    // ✅ رسالة نجاح
                    TempData["SuccessMessage"] = "Booking created successfully 🎉";
                    return RedirectToAction("GetAll");
                }

                ViewBag.error = result.Item2;
            }

            ViewBag.Tables = new SelectList(_tableService.GetAllActiveTables(), "Id", "TableNumber");
            return View(bookingVM);
        }


        //=========================================================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _bookingService.GetById(id);
            if (!result.Item1) // لو حصل خطأ
            {
                ViewBag.error = result.Item2;
                return RedirectToAction("GetAll");
            }

            ViewBag.Tables = new SelectList(_tableService.GetAllActiveTables(), "Id", "TableNumber");
            return View(result.Item3);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBookingVM editBookingVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ViewBag.error = "You must be logged in to edit a booking.";
                    return View(editBookingVM);
                }

                // ✅ اربط الـ CustomerId تلقائي
                editBookingVM.CustomerId = user.Id;

                var result = _bookingService.Edit(id, editBookingVM);
                if (result.Item1)  // النجاح
                    return RedirectToAction("GetAll");

                ViewBag.error = result.Item2;
            }

            ViewBag.Tables = new SelectList(_tableService.GetAllActiveTables(), "Id", "TableNumber");
            return View(editBookingVM);
        }

        //=========================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // رجع الحجوزات الخاصة باليوزر فقط
            var result = _bookingService.GetByCustomerId(user.Id);

            if (!result.Item1)
                ViewBag.error = result.Item2;

            return View(result.Item3);
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

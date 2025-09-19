namespace RestoPL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TableController : Controller
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        // عرض كل الطاولات
        [HttpGet]
        public IActionResult Index()
        {
            var tables = _tableService.GetAll();
            return View(tables.Select(t => new GetAllTableVM
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                Capacity = t.Capacity,
                IsActive = t.IsActive
            }).ToList());
        }

        // إنشاء طاولة
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateTableVM tableVM)
        {
            if (!ModelState.IsValid) return View(tableVM);

            var (hasError, message) = _tableService.Create(tableVM);
            if (hasError)
            {
                ViewBag.error = message;
                return View(tableVM);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        // تعديل طاولة
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var table = _tableService.GetById(id);
            if (table == null) return NotFound();

            return View(new EditTableVM
            {
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                IsActive = table.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditTableVM tableVM)
        {
            if (!ModelState.IsValid) return View(tableVM);

            var (hasError, message) = _tableService.Edit(id, tableVM);
            if (hasError)
            {
                ViewBag.error = message;
                return View(tableVM);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        // حذف طاولة
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var table = _tableService.GetById(id);
            if (table == null) return NotFound();

            return View(new GetAllTableVM
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                IsActive = table.IsActive
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _tableService.Delete(id);

            TempData["Success"] = result ? "Table deleted successfully" : "Failed to delete table";
            return RedirectToAction(nameof(Index));
        }
    }
}

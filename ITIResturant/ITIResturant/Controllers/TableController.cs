


namespace RestoPL.Controllers
{
    public class TableController : Controller
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        //=========================================================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTableVM tableVM)
        {
            if (ModelState.IsValid)
            {
                var result = _tableService.Create(tableVM);
                if (!result.Item1)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.error = result.Item2;
                }
            }
            return View(tableVM);
        }

        //=========================================================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var table = _tableService.GetById(id);
            if (table == null)
            {
                ViewBag.error = "Table not found";
                return RedirectToAction("GetAll");
            }

            var tableVM = new EditTableVM
            {
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                IsActive = table.IsActive
            };

            return View(tableVM);
        }

        [HttpPost]
        public IActionResult Edit(int id, EditTableVM tableVM)
        {
            if (ModelState.IsValid)
            {
                var result = _tableService.Edit(id, tableVM);
                if (!result.Item1)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.error = result.Item2;
                }
            }
            return View(tableVM);
        }

        //=========================================================
        [HttpGet]
        public IActionResult GetAll()
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

        //=========================================================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _tableService.Delete(id);
            if (result)
            {
                return RedirectToAction("GetAll");
            }
            else
            {
                ViewBag.error = "Failed to delete table.";
                return RedirectToAction("GetAll");
            }
        }
    }
}

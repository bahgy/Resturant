

namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Admin only
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            var (hasError, message, categories) = await _categoryService.GetAllAsync();
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return View(new List<CategoryVM>());
            }
            return View(categories);
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var (hasError, message, category) = await _categoryService.GetByIdAsync(id);
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create() => View();

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var (hasError, message) = await _categoryService.CreateAsync(model);
            if (hasError)
            {
                ModelState.AddModelError(nameof(model.Name), message ?? "Error creating category");
                return View(model);
            }

            TempData["SuccessMessage"] = "Category created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var (hasError, message, category) = await _categoryService.GetByIdAsync(id);
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var (hasError, message) = await _categoryService.UpdateAsync(model);
            if (hasError)
            {
                ModelState.AddModelError(nameof(model.Name), message ?? "Error updating category");
                return View(model);
            }

            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var (hasError, message, category) = await _categoryService.GetByIdAsync(id);
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (hasError, message) = await _categoryService.DeleteAsync(id);
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}



namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // restrict only admins

    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: /Admin/Products
        public async Task<IActionResult> Index()
        {
            var (hasError, message, products) = await _productService.GetAllAsync();
            if (hasError)
                TempData["ErrorMessage"] = message;

            return View(products ?? new List<ProductVM>());
        }

        // GET: /Admin/Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var (hasError, message, product) = await _productService.GetByIdAsync(id);
            if (hasError)
                return NotFound(message);

            return View(product);
        }

        // GET: /Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            var (hasError, message, categories) = await _categoryService.GetAllAsync();
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                categories = new List<CategoryVM>();
            }

            ViewBag.Categories = new SelectList(categories, "Id", "Name"); // key = Id, display = Name
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVM model)
        {
            if (!ModelState.IsValid)
            {
                // reload categories if validation fails
                var (hasError, message, categories) = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            var (error, errorMessage) = await _productService.CreateAsync(model);
            if (error)
            {
                ModelState.AddModelError("", errorMessage ?? "Error creating product");
                var (hasError, message, categories) = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }
            TempData["SuccessMessage"] = "Product created successfully.";
            return RedirectToAction(nameof(Index));
        }


        // GET: /Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int id)
{
    var (hasError, message, product) = await _productService.GetByIdAsync(id);
    if (hasError || product == null)
        return NotFound(message);

    var editModel = new EditProductVM
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        ImageUrl = product.ImageUrl,
        IsActive = product.IsActive,
        CategoryId = product.Id 
    };

    var (catError, catMsg, categories) = await _categoryService.GetAllAsync();
    ViewBag.Categories = new SelectList(categories, "Id", "Name", product.Id);

    return View(editModel);
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductVM model)
        {
            if (!ModelState.IsValid)
            {
                var (catError, catMsg, categories) = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            var (hasError, message) = await _productService.UpdateAsync(model);
            if (hasError)
            {
                ModelState.AddModelError("", message ?? "Error updating product.");
                var (catError, catMsg, categories) = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            TempData["SuccessMessage"] = "Product updated successfully.";
            return RedirectToAction(nameof(Index));
        }


        // GET: /Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var (hasError, message, product) = await _productService.GetByIdAsync(id);
            if (hasError)
                return NotFound(message);

            return View(product);
        }

        // POST: /Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (hasError, message) = await _productService.DeleteAsync(id);
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}

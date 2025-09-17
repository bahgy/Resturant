
using Microsoft.EntityFrameworkCore;
using Restaurant.BLL.ModelVMUserVM;

namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Restrict to Admins
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Admin/Users
        public async Task<IActionResult> Index()
        {
            var (hasError, message, users) = await _userService.GetAllUsersAsync();
            if (hasError) TempData["Error"] = message;
            return View(users);
        }

        // GET: /Admin/Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var (hasError, message, user) = await _userService.GetUserByIdAsync(id);
            if (hasError || user == null) return NotFound(message);
            return View(user);
        }

        // GET: /Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserVM UserVM)
        {
            if (!ModelState.IsValid) return View(UserVM);

            var (hasError, message, _) = await _userService.CreateUserAsync(UserVM);
            if (hasError)
            {
                ModelState.AddModelError("", message ?? "Error creating user");
                return View(UserVM);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var (hasError, message, user) = await _userService.GetUserByIdAsync(id);
            if (hasError || user == null) return NotFound(message);
            return View(user);
        }

        // POST: /Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserVM UserVM)
        {
            if (id != UserVM.Id) return BadRequest();
            if (!ModelState.IsValid) return View(UserVM);

            var (hasError, message, user) = await _userService.UpdateUserAsync(UserVM);
            if (hasError)
            {
                ModelState.AddModelError("", message ?? "Error updating user");
                return View(UserVM);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var (hasError, message, user) = await _userService.GetUserByIdAsync(id);
            if (hasError || user == null) return NotFound(message);
            return View(user);
        }

        // POST: /Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (hasError, message, _) = await _userService.DeleteUserAsync(id);
            if (hasError)
            {
                TempData["Error"] = message;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        

    }
}
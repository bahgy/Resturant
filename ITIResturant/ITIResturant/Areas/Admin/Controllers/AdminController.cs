

using Restaurant.PL.Filters;

namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var (_, _, users) = await _userService.GetAllUsersAsync();

            var dashboard = new AdminDashboardVM
            {
                TotalUsers = users?.Count() ?? 0
            };

            return View(dashboard);
        }
    }

    
}

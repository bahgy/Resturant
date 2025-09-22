

namespace ITIResturant.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMenuService _menuService;

        public HomeController(IMenuService _MenuService)
        {
            _menuService = _MenuService;
        }

        public async Task<IActionResult> Index()
        {
            var menu = await _menuService.GetMenuAsync();
            return View(menu); 
        }

        public async Task<IActionResult> Menu()
        {
            var menu = await _menuService.GetMenuAsync();
            return View(menu); 
        }
    }
}

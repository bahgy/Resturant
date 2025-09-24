

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
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }

           
        public IActionResult Events()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }
}

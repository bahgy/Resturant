

using Restaurant.BLL.ModelVM.HomeIndexVm;

namespace ITIResturant.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IReviewService _reviewService;

        public HomeController(IMenuService menuService, IReviewService reviewService)
        {
            _menuService = menuService;
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var menu = await _menuService.GetMenuAsync();
            var reviews = await _reviewService.GetAllAsync();

            var vm = new HomeIndexVm
            {
                Menu = menu,
                Reviews = reviews
            };

            return View(vm);
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

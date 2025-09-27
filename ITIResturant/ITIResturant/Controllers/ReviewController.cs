using Restaurant.BLL.ModelVM.Review;
namespace Restaurant.PL.Controllers
{
    [Authorize(Roles = "Customer")]
    [ServiceFilter(typeof(ValidateUserExistsFilter))]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<AppUser> _userManager;

        public ReviewController(IReviewService reviewService, UserManager<AppUser> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ReviewCreateVm());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ReviewCreateVm model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    string fullName = $"{user.FirstName} {user.LastName}";
                    await _reviewService.AddAsync(fullName, model);
                }


                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}


using ResetPasswordVM = Restaurant.BLL.ModelVMProfileVM.ResetPasswordVM;

namespace Restaurant.PL.Controllers
{
    [Authorize] // Only logged-in users
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly UserManager<AppUser> _userManager;
        public ProfileController(IProfileService profileService, UserManager<AppUser> userManager)
        {
            _profileService = profileService;
            _userManager = userManager;
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveTab"] = "Index";
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var user = await _profileService.GetUserProfileAsync(userId);
            if (user == null) return NotFound();

            // Map Customer (AppUser) -> UpdateProfileVM
            var model = new UpdateProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(model);
        }


        // GET: Profile/Update
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var user = await _profileService.GetUserProfileAsync(userId);
            if (user == null) return NotFound();

            
            var model = new UpdateProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(model);
        }

        // POST: Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateProfileVM model)
        {
            if (!ModelState.IsValid) return View("Index", model);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var user = await _profileService.GetUserProfileAsync(userId);
            if (user == null) return NotFound();

            string? confirmationLink = null;
            if (user.Email != model.Email)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = token }, protocol: Request.Scheme);
            }

            var (Success, ErrorMessage) = await _profileService.UpdateProfileAsync(userId, model, confirmationLink);

            if (Success)
            {
                TempData["SuccessMessage"] = ErrorMessage ?? "Your profile has been updated successfully!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = ErrorMessage;
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return View("Index", model);
        }




        // GET: Profile/ResetPassword
        public IActionResult ResetPassword()
        {
            ViewData["ActiveTab"] = "ResetPassword";
            return View(new ResetPasswordVM());
        }

        // POST: Profile/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var (Success, ErrorMessage) = await _profileService.ResetPasswordAsync(userId, model);

            if (Success)
            {
                TempData["Success"] = "Password changed successfully.";
                return RedirectToAction(nameof(ResetPassword));
            }
            TempData["Error"] = ErrorMessage;
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return View(model);
        }
        public IActionResult OrderHistory()
        {
            ViewData["ActiveTab"] = "OrderHistory";
            return View();
        }
        public IActionResult CurrentBookings()
        {
            ViewData["ActiveTab"] = "CurrentBookings";
            return View();
        }
    }
}

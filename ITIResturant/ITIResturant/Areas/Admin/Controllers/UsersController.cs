

namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Restrict to Admins
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly EmailSender _emailSender;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public UsersController(IUserService userService, UserManager<AppUser> userManager, EmailSender emailSender, SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
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

            var (hasError, message, createdUser) = await _userService.CreateUserAsync(UserVM);
            if (hasError)
            {
                ModelState.AddModelError("", message ?? "Error creating user");
                return View(UserVM);
            }

            //  Send confirmation email
            var appUser = await _userManager.FindByEmailAsync(createdUser.Email);
            // After user is created in CreateUserAsync (UserService)
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var confirmationLink = Url.Action("ConfirmEmail", "Users",
                new { area = "Admin",  userId = appUser.Id, token = token },
                protocol: HttpContext.Request.Scheme);
            // Send email
            await _emailSender.SendEmailAsync(appUser.Email, "Confirm your email",
    $"<p>Please confirm your email by clicking the link below:</p>" +
    $"<a href='{confirmationLink}'>Confirm Email</a>");

            TempData["SuccessMessage"] = "User created successfully. A confirmation email has been sent.";
            return RedirectToAction(nameof(Index));

        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            if (userId == 0 || string.IsNullOrEmpty(token))
                return BadRequest("Invalid confirmation request.");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound("User not found.");

            // Decode token
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                if (user is Customer customer)
                {
                    customer.EmailVerified = true;
                    await _userManager.UpdateAsync(customer);
                }
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            return View("ConfirmEmailFailure");
        }




        // GET: /Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var (hasError, message, user) = await _userService.GetUserByIdAsync(id);
            if (hasError || user == null) return NotFound(message);
            return View(user);
        }

        // POST: /Admin/Users/Edit/5
        // POST: /Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserVM UserVM)
        {
            if (id != UserVM.Id) return BadRequest();
            if (!ModelState.IsValid) return View(UserVM);

            // Get existing user before updating (to compare email later)
            var (hasError, message, existingUserVM) = await _userService.GetUserByIdAsync(id);
            if (hasError || existingUserVM == null)
            {
                ModelState.AddModelError("", message ?? "User not found");
                return View(UserVM);
            }

            var oldEmail = existingUserVM.Email;

            // Update user
            var (updateError, updateMessage, updatedUserVM) = await _userService.UpdateUserAsync(UserVM);
            if (updateError || updatedUserVM == null)
            {
                ModelState.AddModelError("", updateMessage ?? "Error updating user");
                return View(UserVM);
            }

            // Fetch the real Identity user (AppUser) from UserManager
            var identityUser = await _userManager.FindByIdAsync(updatedUserVM.Id.ToString());
            if (identityUser == null)
            {
                TempData["ErrorMessage"] = "Updated user not found in Identity.";
                return RedirectToAction(nameof(Index));
            }

            // If email changed, reset confirmation and send verification link
            if (!string.Equals(oldEmail, updatedUserVM.Email, StringComparison.OrdinalIgnoreCase))
            {
                identityUser.EmailConfirmed = false; // force re-confirmation
                if (identityUser is Customer customer)
                {
                    customer.EmailVerified = false; // custom field
                }
                await _userManager.UpdateAsync(identityUser);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = identityUser.Id, token = token, area = "" },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(identityUser.Email, "Confirm your email",
                    $"<p>Your email was updated. Please confirm it by clicking the link below:</p>" +
                    $"<a href='{confirmationLink}'>Confirm Email</a>");
                TempData["SuccessMessage"] = "User updated successfully. Verification email sent to the new address.";
            }
            else
            {
                TempData["SuccessMessage"] = "User updated successfully.";
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
            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        

    }
}
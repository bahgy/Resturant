

using ResetPasswordVM = Restaurant.BLL.ModelVMAccountVM.ResetPasswordVM;

namespace Restaurant.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;
        private readonly EmailSender _emailSender;

        public AccountController(
            IAuthService authService,
            ILogger<AccountController> logger,
            EmailSender emailSender)
        {
            _authService = authService;
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var (hasError, result, errorMessage) = await _authService.RegisterUserAsync(model);

                if (hasError)
                {
                    ModelState.AddModelError("", errorMessage);
                    return View(model);
                }

                if (result.Succeeded)
                {
                    var (hasError2, user, errorMessage2) = await _authService.FindUserByEmailAsync(model.Email);

                    if (hasError2)
                    {
                        ModelState.AddModelError("", errorMessage2);
                        return View(model);
                    }

                    if (user != null)
                    {
                        var (hasError3, token, errorMessage3) = await _authService.GenerateEmailConfirmationTokenAsync(user);

                        if (hasError3)
                        {
                            ModelState.AddModelError("", errorMessage3);
                            return View(model);
                        }

                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                            new { userId = user.Id, token = token }, Request.Scheme);

                        await _emailSender.SendEmailAsync(user.Email,
                            "Confirm your email - Restaurant App",
                            $"<p>Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.</p>");

                        TempData["Message"] = "Check your email to confirm your account.";
                        return View("RegisterConfirmation");
                    }
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return View("Error");
            }

            var (hasError, result, errorMessage) = await _authService.ConfirmEmailAsync(userId, token);

            if (hasError)
            {
                _logger.LogError(errorMessage);
                return View("Error");
            }

            if (result.Succeeded)
            {
                var (hasError2, user, errorMessage2) = await _authService.FindUserByIdAsync(userId);

                if (hasError2)
                {
                    _logger.LogError(errorMessage2);
                    return View("Error");
                }

                if (user != null)
                {
                    var (hasError3, errorMessage3) = await _authService.SignInAsync(user, isPersistent: false);

                    if (hasError3)
                    {
                        _logger.LogError(errorMessage3);
                        return View("Error");
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            return View("Error");
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var (hasError, user, errorMessage) = await _authService.FindUserByEmailAsync(model.Email);

            if (hasError || user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var (hasError2, isConfirmed, errorMessage2) = await _authService.IsEmailConfirmedAsync(user);

            if (hasError2)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var (hasError3, token, errorMessage3) = await _authService.GeneratePasswordResetTokenAsync(user);

            if (hasError3)
            {
                _logger.LogError(errorMessage3);
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var resetLink = Url.Action("ResetPassword", "Account",
                new { token, email = user.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email,
                "Reset your password - Restaurant App",
                $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation() => View();

        // GET: /Account/ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (token == null || email == null) return BadRequest("Invalid password reset request.");
            return View(new ResetPasswordVM { Token = token, Email = email });
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var (hasError, result, errorMessage) = await _authService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);

            if (hasError)
            {
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }

            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation() => View();

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return View(model);
            }

            var (hasError, user, errorMessage) = await _authService.FindUserByEmailAsync(model.Email);

            if (hasError)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            

            var (hasError2, result, errorMessage2) = await _authService.PasswordSignInAsync(
                user.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (hasError2)
            {
                ModelState.AddModelError("", errorMessage2);
                return View(model);
            }
            
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "You need to confirm your email to log in.");
                return View(model);
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var (hasError, errorMessage) = await _authService.SignOutAsync();

            if (hasError)
            {
                _logger.LogError(errorMessage);
            }

            return RedirectToAction("Index", "Home");
        }

        // External login: Google and Facebook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _authService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                TempData["ErrorMessage"] = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var (hasError, info, errorMessage) = await _authService.GetExternalLoginInfoAsync();

            if (hasError || info == null)
            {
                TempData["ErrorMessage"] = hasError ? errorMessage : "External login information not available.";
                return RedirectToAction(nameof(Login));
            }

            var (hasError2, result, errorMessage2) = await _authService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (hasError2)
            {
                TempData["ErrorMessage"] = errorMessage2;
                return RedirectToAction(nameof(Login));
            }

            if (result.Succeeded)
                return LocalRedirect(returnUrl);

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Email not provided by external provider.";
                return RedirectToAction(nameof(Login));
            }

            var (hasError3, user, errorMessage3) = await _authService.FindUserByEmailAsync(email);

            if (hasError3)
            {
                TempData["ErrorMessage"] = errorMessage3;
                return RedirectToAction(nameof(Login));
            }

            if (user == null)
            {
                var (hasError4, createdUser, errorMessage4) = await _authService.CreateExternalLoginUserAsync(email);

                if (hasError4 || createdUser == null)
                {
                    TempData["ErrorMessage"] = hasError4 ? errorMessage4 : "Failed to create user from external login.";
                    return RedirectToAction(nameof(Login));
                }
                user = createdUser;
            }

            var (hasError5, addLoginResult, errorMessage5) = await _authService.AddLoginAsync(user, info);

            if (hasError5 || !addLoginResult.Succeeded)
            {
                TempData["ErrorMessage"] = hasError5 ? errorMessage5 : "Failed to link external login to account.";
                return RedirectToAction(nameof(Login));
            }

            var (hasError6, errorMessage6) = await _authService.SignInAsync(user, isPersistent: false);

            if (hasError6)
            {
                TempData["ErrorMessage"] = errorMessage6;
                return RedirectToAction(nameof(Login));
            }

            return LocalRedirect(returnUrl);
        }
    }
}
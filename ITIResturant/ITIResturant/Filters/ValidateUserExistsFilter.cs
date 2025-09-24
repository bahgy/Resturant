

namespace Restaurant.PL.Filters
{
    public class ValidateUserExistsFilter: IAsyncActionFilter
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ValidateUserExistsFilter(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    // Sign out
                    await _signInManager.SignOutAsync();

                    // Redirect to login
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                    return;
                }
            }

            await next();
        }
    }
}

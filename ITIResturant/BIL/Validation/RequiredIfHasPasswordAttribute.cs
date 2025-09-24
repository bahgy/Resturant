

using System.Security.Claims;

namespace Restaurant.BLL.Validation
{
    public class RequiredIfHasPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor))!;
            var userManager = (UserManager<AppUser>)validationContext.GetService(typeof(UserManager<AppUser>))!;

            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return new ValidationResult("User not found.");
            }

            var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            if (user == null)
            {
                return new ValidationResult("User not found.");
            }

            var hasPassword = userManager.HasPasswordAsync(user).GetAwaiter().GetResult();

            // If user has password → require field
            if (hasPassword && string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required.");
            }

            return ValidationResult.Success;
        }
    }
}

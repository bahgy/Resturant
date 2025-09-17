

namespace Restaurant.BLL.Abstraction
{
    public interface IAuthService
    {
        Task<(bool hasError, IdentityResult result, string? errorMessage)> RegisterUserAsync(RegisterVM model);
        Task<(bool hasError, string token, string? errorMessage)> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<(bool hasError, IdentityResult result, string? errorMessage)> ConfirmEmailAsync(string userId, string token);
        Task<(bool hasError, AppUser user, string?   errorMessage)> FindUserByEmailAsync(string email);
        Task<(bool hasError, bool isConfirmed, string?       errorMessage)> IsEmailConfirmedAsync(AppUser user);
        Task<(bool hasError, string token, string? errorMessage)> GeneratePasswordResetTokenAsync(AppUser user);
        Task<(bool hasError, IdentityResult result, string? errorMessage)> ResetPasswordAsync(string email, string token, string newPassword);
        Task<(bool hasError, SignInResult result, string? errorMessage)> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockoutOnFailure);
        Task<(bool hasError, string? errorMessage)> SignInAsync(AppUser user, bool isPersistent);
        Task<(bool hasError, string? errorMessage)> SignOutAsync();
        Task<(bool hasError, ExternalLoginInfo info, string? errorMessage)> GetExternalLoginInfoAsync();
        Task<(bool hasError, SignInResult result, string? errorMessage)> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
        Task<(bool hasError, IdentityResult result, string? errorMessage)> AddLoginAsync(AppUser user, ExternalLoginInfo loginInfo);
        Task<(bool hasError, AppUser user, string? errorMessage)> FindUserByIdAsync(string userId);
        Task<(bool hasError, AppUser user, string? errorMessage)> CreateExternalLoginUserAsync(string email);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
    }
}
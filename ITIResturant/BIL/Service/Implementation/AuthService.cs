

namespace Restaurant.BLL.Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool hasError, IdentityResult result, string? errorMessage)> RegisterUserAsync(RegisterVM model)
        {
            try
            {
                AppUser user;

                if (model.IsAdmin)
                {
                    user = new Admin
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        UserType = UserTypeEnum.Admin,
                        EmailConfirmed = true // Admin email is confirmed by default
                    };
                }
                else
                {
                    user = new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        UserType = UserTypeEnum.Customer,

                    };
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddToRoleAsync(user, model.IsAdmin ? "Admin" : "Customer");
                return (false, result, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, IdentityResult.Failed(), $"Registration failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, string token, string? errorMessage)> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return (false, token, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, string.Empty, $"Token generation failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, IdentityResult result, string? errorMessage)> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return (true, IdentityResult.Failed(), "User not found.");
                }
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    if (user is Customer customer){
                        await _userManager.UpdateAsync(customer);
                    }
                }
                return (false, result, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, IdentityResult.Failed(), $"Email confirmation failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, AppUser user, string? errorMessage)> FindUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                return (false, user, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, null, $"User lookup failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, bool isConfirmed, string? errorMessage)> IsEmailConfirmedAsync(AppUser user)
        {
            try
            {
                var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                return (false, isConfirmed, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, false, $"Email confirmation check failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, string token, string? errorMessage)> GeneratePasswordResetTokenAsync(AppUser user)
        {
            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                return (false, token, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, string.Empty, $"Password reset token generation failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, IdentityResult result, string? errorMessage)> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return (true, IdentityResult.Failed(), "User not found.");
                }

                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                return (false, result, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, IdentityResult.Failed(), $"Password reset failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, SignInResult result, string? errorMessage)> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockoutOnFailure)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userName, password, rememberMe, lockoutOnFailure);
                return (false, result, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, SignInResult.Failed, $"Login failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, string? errorMessage)> SignInAsync(AppUser user, bool isPersistent)
        {
            try
            {
                await _signInManager.SignInAsync(user, isPersistent);
                return (false, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, $"Sign in failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, string? errorMessage)> SignOutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return (false, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, $"Sign out failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, ExternalLoginInfo info, string? errorMessage)> GetExternalLoginInfoAsync()
        {
            try
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                return (false, info, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, null, $"External login info retrieval failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, SignInResult result, string? errorMessage)> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            try
            {
                var result = await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
                return (false, result, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, SignInResult.Failed, $"External login failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, IdentityResult result, string? errorMessage)> AddLoginAsync(AppUser user, ExternalLoginInfo loginInfo)
        {
            try
            {
                var result = await _userManager.AddLoginAsync(user, loginInfo);
                return (false, result, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, IdentityResult.Failed(), $"Adding external login failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, AppUser user, string? errorMessage)> FindUserByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                return (false, user, string.Empty);
            }
            catch (Exception ex)
            {
                return (true, null, $"User lookup by ID failed: {ex.Message}");
            }
        }

        public async Task<(bool hasError, AppUser user, string? errorMessage)> CreateExternalLoginUserAsync(string email)
        {
            try
            {
                var user = new Customer
                {
                    UserName = email,
                    Email = email,
                    FirstName = email.Split('@')[0],
                    LastName = "User",
                    UserType = UserTypeEnum.Customer,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "Customer");

                return result.Succeeded ?
                    (false, user, string.Empty) :
                    (true, null, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            catch (Exception ex)
            {
                return (true, null, $"External user creation failed: {ex.Message}");
            }
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            try
            {
                return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            }
            catch (Exception ex)
            {
                throw new Exception($"External authentication configuration failed: {ex.Message}", ex);
            }
        }
    }
}
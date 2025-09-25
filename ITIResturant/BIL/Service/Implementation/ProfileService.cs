

using Microsoft.AspNetCore.Identity;

namespace Restaurant.BLL.Service.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ProfileService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
        }

        public async Task<AppUser?> GetUserProfileAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateProfileAsync(
            string userId,
            UpdateProfileVM model,
            string? emailConfirmationLink = null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            bool emailChanged = user.Email != model.Email;

            // Map fields from VM → User (Username comes from email before '@')
            _mapper.Map(model, user);

            if (emailChanged)
            {
                // Check duplicate email
                if (await _userManager.FindByEmailAsync(model.Email) != null)
                {
                    return (false, "Email is already taken.");
                }

                user.Email = model.Email;
                user.UserName = model.UserName;
                user.EmailConfirmed = false;

            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            // Handle email confirmation
            if (emailChanged && !string.IsNullOrEmpty(emailConfirmationLink))
            {
                var emailSender = new EmailSender(_config);
                await emailSender.SendEmailAsync(user.Email, "Confirm your new email",
                    $"Please confirm your new email by clicking <a href='{emailConfirmationLink}'>here</a>.");

                return (true, "Your profile was updated. Please check your new email to confirm it.");
            }
            else if (!emailChanged && !user.EmailConfirmed)
            {
                return (true, "Your profile was updated. Please confirm your email.");
            }

            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(
    string userId,
    ResetPasswordVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                // Normal flow (requires current password)
                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!passwordCheck)
                    return (false, "Current password is incorrect.");

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                return result.Succeeded
                    ? (true, null)
                    : (false, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            else
            {
                // External user → just add a password (no current password check)
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                return result.Succeeded
                    ? (true, null)
                    : (false, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }








    }
}

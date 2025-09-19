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

        public async Task<(bool Success, string? ErrorMessage)> UpdateProfileAsync(string userId, UpdateProfileVM model, string? emailConfirmationLink = null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            bool emailChanged = user.Email != model.Email;

            // Map other fields
            user.FirstName = model.FirstName?.Trim();
            user.LastName = model.LastName?.Trim();
            user.PhoneNumber = model.PhoneNumber?.Trim();
            user.Address = model.Address?.Trim();

            if (emailChanged)
            {
                if(await _userManager.FindByEmailAsync(model.Email) != null)
                {
                    return (false, "Email is already taken.");
                }
                user.Email = model.Email;
                user.UserName = model.Email;
                user.EmailConfirmed = false;
                if (user is Customer customer)
                {
                    customer.EmailVerified = false;
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            // If email changed, send confirmation email
            if (emailChanged && !string.IsNullOrEmpty(emailConfirmationLink))
            {
                var emailSender = new EmailSender(_config);
                await emailSender.SendEmailAsync(user.Email, "Confirm your new email",
                    $"Please confirm your new email by clicking <a href='{emailConfirmationLink}'>here</a>.");

                return (true, "Your profile was updated. Please check your new email to confirm it.");
            }
            // if email not changed but the current email not confirmed
            else if (!emailChanged && !user.EmailConfirmed)
            {
                return (true, "Your profile was updated. Please confirm your email.");
            }
            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(string userId, ResetPasswordVM model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            // check current password
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!passwordCheck)
                return (false, "Current password is incorrect.");

            // reset password
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
                return (true, null);

            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

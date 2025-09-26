

namespace Restaurant.BLL.Services.Interfaces
{
    public interface IProfileService
    {
        Task<AppUser?> GetUserProfileAsync(string userId);
        Task<(bool Success, string? ErrorMessage)> UpdateProfileAsync(string userId, UpdateProfileVM model, string? emailConfirmationLink = null);
        Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(string userId, ResetPasswordVM model);
    }
}

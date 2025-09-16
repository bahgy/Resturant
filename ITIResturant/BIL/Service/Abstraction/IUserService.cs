public interface IUserService
{
    Task<(bool hasError, string? message, UserVM? user)> GetUserByIdAsync(int id);
    Task<(bool hasError, string? message, IEnumerable<UserVM>? users)> GetAllUsersAsync();
    Task<(bool hasError, string? message, UserVM? user)> CreateUserAsync(UserVM user);
    Task<(bool hasError, string? message, UserVM? user)> UpdateUserAsync(UserVM user);
    Task<(bool hasError, string? message, bool isDeleted)> DeleteUserAsync(int id);

}

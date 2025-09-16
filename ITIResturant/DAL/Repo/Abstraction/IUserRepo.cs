namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IUserRepo
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateUserAsync(AppUser user);
    }
}

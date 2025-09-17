

namespace Restaurant.DAL.Repo.Implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly RestaurantDbContext DataBase;

        public UserRepo(RestaurantDbContext context)
        {
            DataBase = context;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
            => await DataBase.Users.ToListAsync();

        public async Task<AppUser?> GetUserByIdAsync(int id)
            => await DataBase.Users.FindAsync(id);

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await DataBase.Users.FindAsync(id);
            if (user == null) return false;
            DataBase.Users.Remove(user);
            await DataBase.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(AppUser user)
        {
            DataBase.Users.Update(user);
            await DataBase.SaveChangesAsync();
            return true;
        }
    }
}

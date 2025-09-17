
namespace Restaurant.DAL.Repo.Implementation
{
    public class CustomerRepo:ICustomerRepo
    {
        private readonly RestaurantDbContext DataBase;

        public CustomerRepo(RestaurantDbContext context)
        {
            DataBase = context;
        }

        public Task<bool> ExistsAsync(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}

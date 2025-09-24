
namespace Restaurant.DAL.Repo.Implementation
{
    public class CustomerRepo:ICustomerRepo
    {
        private readonly RestaurantDbContext DataBase;

        public CustomerRepo(RestaurantDbContext context)
        {
            DataBase = context;
        }

        public async Task<bool> ExistsAsync(int customerId)
        {
            return await DataBase.Customers.AnyAsync(c => c.Id == customerId);
        }
    }
}

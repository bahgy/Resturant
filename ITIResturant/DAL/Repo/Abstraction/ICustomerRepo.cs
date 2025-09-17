
namespace Restaurant.DAL.Repo.Abstraction
{
    public interface ICustomerRepo
    {
        Task<bool> ExistsAsync(int customerId);
    }
}

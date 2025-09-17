
namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IProductRepo
    {
        Task<Product> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

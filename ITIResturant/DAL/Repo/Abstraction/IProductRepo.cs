
namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IProductRepo
    {
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}

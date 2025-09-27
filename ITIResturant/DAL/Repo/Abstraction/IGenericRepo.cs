
namespace Rsturant.DAL.Repo.Abstraction
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);

        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
        Task<int> SaveAsync();
    }

}

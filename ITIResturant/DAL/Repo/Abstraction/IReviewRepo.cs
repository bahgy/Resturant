
namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IReviewRepo
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(int id);

    }
}

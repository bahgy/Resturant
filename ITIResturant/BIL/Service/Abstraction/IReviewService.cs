using Restaurant.BLL.ModelVM.Review;

namespace Restaurant.BLL.Service.Abstraction
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewVm>> GetAllAsync();
        Task<ReviewVm?> GetByIdAsync(int id);
        Task AddAsync(string userFullName, ReviewCreateVm model);
        Task DeleteAsync(int id);
    }
}

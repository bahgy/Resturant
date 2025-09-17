
namespace Restaurant.BLL.Service.Abstraction
{
    public interface IFeedbackService
    {

        Task<IEnumerable<GetAllFeedbackVM>> GetAllAsync();
        Task<GetAllFeedbackVM> GetByIdAsync(int id);
        Task AddAsync(CreateFeedbackVM feedbackVM);
        Task UpdateAsync(UpdateFeedbackVM feedbackVM);
        Task DeleteAsync(int id);

       
    }
}

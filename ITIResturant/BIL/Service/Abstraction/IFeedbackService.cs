using Restaurant.BLL.Model_VM.Feedback;
using Restaurant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

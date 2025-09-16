using BIL.Model_VM.Feedback;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.Service.Abstraction
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

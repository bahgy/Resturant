using Restaurant.BLL.Model_VM.Feedback;
using Restaurant.BLL.Service.Abstraction;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace Restaurant.BLL.Service.Impelementation
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepo _repository;

        public FeedbackService(IFeedbackRepo repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(CreateFeedbackVM feedbackVM)
        {
            try
            {
                Feedback feedback = new()
                {
                    Comment = feedbackVM.Comment,
                    Rating = feedbackVM.Rating,
                    SubmittedDate = DateTime.Now,
                    CustomerId = feedbackVM.CustomerId,
                    OrderId = feedbackVM.OrderId
                };

                await Task.Run(() => _repository.Add(feedback));
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding feedback.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await Task.Run(() => _repository.Delete(id));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting feedback with ID {id}.", ex);
            }
        }


        public async Task<IEnumerable<GetAllFeedbackVM>> GetAllAsync()
        {
            try
            {
                return await Task.Run(() =>
                    _repository.GetAll().Select(f => new GetAllFeedbackVM
                    {
                        Id = f.Id,
                        Comment = f.Comment,
                        Rating = f.Rating,
                        SubmittedDate = f.SubmittedDate

                    }).ToList()
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving feedback list.", ex);
            }
        }

        public async Task<GetAllFeedbackVM> GetByIdAsync(int id)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var f = _repository.GetById(id);
                    if (f == null) return null;

                    return new GetAllFeedbackVM
                    {
                        Id = f.Id,
                        Comment = f.Comment,
                        Rating = f.Rating,
                        SubmittedDate = f.SubmittedDate

                    };
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving feedback with ID {id}.", ex);
            }
        }

        public async Task UpdateAsync(UpdateFeedbackVM feedbackVM)
        {
            try
            {
                var feedback = new Feedback
                {
                    Id = feedbackVM.Id,
                    Comment = feedbackVM.Comment,
                    Rating = feedbackVM.Rating,
                    SubmittedDate = feedbackVM.SubmittedDate,
                    CustomerId = feedbackVM.CustomerId,
                    OrderId = feedbackVM.OrderId
                };

                await Task.Run(() => _repository.Update(feedback));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating feedback with ID {feedbackVM.Id}.", ex);
            }
        }
    }


}


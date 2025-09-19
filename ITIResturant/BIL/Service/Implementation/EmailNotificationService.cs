

namespace Restaurant.BLL.Service.Implementation
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IEmailNotificationRepo _repository;

        public EmailNotificationService(IEmailNotificationRepo repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(EmailNotification notification)
        {
            try
            {
                await Task.Run(() => _repository.AddAsync(notification));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding Email Notification", ex);
            }
        }

        public (bool, string) Create(CreateEmailNotificationVM createEmailNotificationVM)
        {
            try
            {
                var notification = new EmailNotification
                {
                    ToAddress = createEmailNotificationVM.ToAddress,
                    Subject = createEmailNotificationVM.Subject,
                    Body = createEmailNotificationVM.Body,
                    SentTime = DateTime.Now,
                    status = "Pending",
                    Type = createEmailNotificationVM.Type,
                    CustomerId = createEmailNotificationVM.CustomerId
                };

                _repository.Add(notification);

                return (true, "Email notification created successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Failed to create Email Notification: {ex.Message}");
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
                throw new Exception("Error while deleting Email Notification", ex);
            }
        }

        public async Task<IEnumerable<EmailNotification>> GetAllAsync()
        {
            try
            {
                return await Task.Run(() => _repository.GetAllAsync());
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving all Email Notifications", ex);
            }
        }

        public async Task<EmailNotification> GetByIdAsync(int id)
        {
            try
            {
                return await Task.Run(() => _repository.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving Email Notification with ID {id}", ex);
            }
        }

        public async Task UpdateAsync(EmailNotification notification)
        {
            try
            {
                await Task.Run(() => _repository.UpdateAsync(notification));
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating Email Notification", ex);
            }
        }
    }
}

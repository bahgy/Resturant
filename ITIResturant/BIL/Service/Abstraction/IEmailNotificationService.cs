using Restaurant.BLL.Model_VM.EmailNotification;
using Restaurant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Service.Abstraction
{
    public interface IEmailNotificationService
    {
        Task<IEnumerable<EmailNotification>> GetAllAsync();
        (bool, string) Create(CreateEmailNotificationVM createEmailNotificationVM);
        Task<EmailNotification> GetByIdAsync(int id);
        Task AddAsync(EmailNotification notification);
        Task UpdateAsync(EmailNotification notification);
        Task DeleteAsync(int id);

    }
}

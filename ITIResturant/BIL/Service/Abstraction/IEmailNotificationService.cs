using BIL.Model_VM.EmailNotification;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.Service.Abstraction
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

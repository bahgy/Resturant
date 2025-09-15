using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.Abstraction
{
    public interface IEmailNotificationRepo
    {

        /*  Task<IEnumerable<EmailNotification>> GetAllAsync();
          Task<EmailNotification> GetByIdAsync(int id);
          Task AddAsync(EmailNotification notification);
          Task UpdateAsync(EmailNotification notification);
          Task DeleteAsync(int id);
          void Add(EmailNotification notification);
          void Delete(int id);
          Task<IEnumerable<EmailNotification>>? GetAll();
          Task<EmailNotification>? GetById(int id);
          void Update(EmailNotification notification); */


        Task AddAsync(EmailNotification notification);
        Task UpdateAsync(EmailNotification notification);
        Task DeleteAsync(int id);
        Task<IEnumerable<EmailNotification>> GetAllAsync();
        Task<EmailNotification?> GetByIdAsync(int id);
        void Add(EmailNotification notification);
        void Delete(int id);
    }
}

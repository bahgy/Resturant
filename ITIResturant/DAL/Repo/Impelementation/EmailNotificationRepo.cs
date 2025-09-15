using DAL.DataBase;
using DAL.Entities;
using DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.Impelementation
{
    public class EmailNotificationRepo : IEmailNotificationRepo
    {
        private readonly ResturantDbContext _context;



        public EmailNotificationRepo(ResturantDbContext context)
        {
            _context = context;
        }

        public void Add(EmailNotification notification)
        {
            throw new NotImplementedException();
        }

        //==============================================================

        public async Task AddAsync(EmailNotification notification)
        {
            try
            {
                await _context.EmailNotifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception("Error while adding EmailNotification", ex);
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        //==============================================================

        public async Task DeleteAsync(int id)
        {
            try
            {
                var notification = await _context.EmailNotifications.FindAsync(id);
                if (notification == null)
                {
                    throw new Exception($"Notification with Id {id} not found");
                }

                _context.EmailNotifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting EmailNotification", ex);
            }
        }

        public Task<IEnumerable<EmailNotification>>? GetAll()
        {
            throw new NotImplementedException();
        }

        //==============================================================

        public async Task<IEnumerable<EmailNotification>> GetAllAsync()
        {
            try
            {
                return await _context.EmailNotifications
                                     .Include(e => e.Customer) // eager load customer
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving all EmailNotifications", ex);
            }
        }

        public Task<EmailNotification>? GetById(int id)
        {
            throw new NotImplementedException();
        }

        //==============================================================


        public async Task<EmailNotification> GetByIdAsync(int id)
        {
            try
            {
                var notification = await _context.EmailNotifications
                                                 .Include(e => e.Customer)
                                                 .FirstOrDefaultAsync(e => e.Id == id);

                if (notification == null)
                {
                    throw new Exception($"Notification with Id {id} not found");
                }

                return notification;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving EmailNotification with Id {id}", ex);
            }
        }

        public void Update(EmailNotification notification)
        {
            throw new NotImplementedException();
        }

        //=================================================================


        public async Task UpdateAsync(EmailNotification notification)
        {
            try
            {
                var existing = await _context.EmailNotifications.FindAsync(notification.Id);
                if (existing == null)
                {
                    throw new Exception($"Notification with Id {notification.Id} not found");
                }

                // Update fields manually
                existing.ToAddress = notification.ToAddress;
                existing.Subject = notification.Subject;
                existing.Body = notification.Body;
                existing.SentTime = notification.SentTime;
                existing.status = notification.status;
                existing.Type = notification.Type;
                existing.CustomerId = notification.CustomerId;

                _context.EmailNotifications.Update(existing);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating EmailNotification", ex);
            }
        }


    }
}

using Restaurant.DAL.Database;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repo.Impelementation
{
    public class FeedbackRepo : IFeedbackRepo
    {
        private readonly RestaurantDbContext _context;

        public FeedbackRepo(RestaurantDbContext context)
        {
            _context = context;
        }

        public void Add(Feedback feedback)
        {
            try
            {
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new feedback.", ex);
            }
        }
        //===========================================================

        public void Delete(int id)
        {
            try
            {
                var feedback = _context.Feedbacks.Find(id);
                if (feedback == null)
                {
                    throw new Exception($"Feedback with ID {id} not found.");
                }

                _context.Feedbacks.Remove(feedback);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting feedback with ID {id}.", ex);
            }
        }
        //============================================================
        public IEnumerable<Feedback> GetAll()
        {

            try
            {
                return _context.Feedbacks
                    .Include(f => f.Customer)
                    .Include(f => f.Order)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving feedback list.", ex);
            }
        }
        //===============================================================

        public Feedback GetById(int id)
        {
            try
            {
                return _context.Feedbacks
                    .Include(f => f.Customer)
                    .Include(f => f.Order)
                    .FirstOrDefault(f => f.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving feedback with ID {id}.", ex);
            }
            
        }

        //============================================================

        public void Update(Feedback feedback)
        {
            try
            {
                _context.Feedbacks.Update(feedback);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating feedback with ID {feedback.Id}.", ex);
            }
        }
    }
    }


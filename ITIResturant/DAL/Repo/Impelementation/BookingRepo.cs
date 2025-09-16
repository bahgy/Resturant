using Restaurant.DAL.Database;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repo.Impelementation
{
    public class BookingRepo : IBookingRepo
    {
        private readonly RestaurantDbContext Db;

        public BookingRepo(RestaurantDbContext context)
        {
            Db = context;
        }

        //============================================================
        public bool Create(Booking booking)
        {
            try
            {
                var result = Db.Bookings.Add(booking);
                Db.SaveChanges();
                return result.Entity.Id > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //============================================================
        public bool Delete(int bookingId)
        {
            try
            {
                var oldBooking = Db.Bookings.FirstOrDefault(a => a.Id == bookingId);
                if (oldBooking != null)
                {
                    Db.Bookings.Remove(oldBooking);
                    Db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //============================================================
        public bool Edit(int bookingId, Booking newBooking)
        {
            try
            {
                var oldBooking = Db.Bookings.FirstOrDefault(a => a.Id == bookingId);
                if (oldBooking != null)
                {
                    oldBooking.Update(
                        newBooking.BookingDate,
                        newBooking.StartTime,
                        newBooking.EndTime,
                        newBooking.NumberOfGuests,
                        newBooking.Status,
                        newBooking.SpecialRequests,
                        newBooking.CustomerId,
                        newBooking.TableId // ✅ استخدم TableId مش TableNumber
                    );

                    Db.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //============================================================
        public List<Booking> GetAll(Expression<Func<Booking, bool>>? filter = null)
        {
            try
            {
                if (filter != null)
                {
                    return Db.Bookings.Where(filter).ToList();
                }
                else
                {
                    return Db.Bookings.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //============================================================
        public Booking GetById(int bookingId)
        {
            try
            {
                return Db.Bookings.FirstOrDefault(a => a.Id == bookingId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

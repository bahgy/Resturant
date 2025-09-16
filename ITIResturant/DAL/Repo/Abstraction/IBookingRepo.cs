using Restaurant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IBookingRepo
    {
        bool Create(Booking booking);
        bool Edit(int bookingId, Booking newBooking);
        List<Booking> GetAll(Expression<Func<Booking, bool>>? filter = null);
        Booking GetById(int bookingId);
        bool Delete(int bookingId);

    }
}

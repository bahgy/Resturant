

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

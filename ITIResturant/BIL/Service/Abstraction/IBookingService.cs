

namespace Restaurant.BLL.Service.Abstraction
{
    public interface IBookingService
    {
        (bool, string) Create(CreateBookingVM createbookingVM);
        (bool, string) Edit(int bookingId, EditBookingVM editBookingVM);
        (bool, string, List<GetAllBookingVM>) GetAll();
        (bool, string, EditBookingVM) GetById(int bookingId);
        bool Delete(int bookingId);

    }
}

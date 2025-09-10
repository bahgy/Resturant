using BIL.Model_VM.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.Service.Abstraction
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

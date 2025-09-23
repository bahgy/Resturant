

using Microsoft.EntityFrameworkCore;

namespace Restaurant.BLL.Service.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly ITableService _tableService;
        public BookingService(IBookingRepo bookingRepo, ITableService tableService)
        {
            _bookingRepo = bookingRepo;
            _tableService = tableService;
        }

        //============================================================
        public (bool, string) Create(CreateBookingVM bookingVM)
        {
            try
            {
                // التحقق من اختيار الطاولة
                if (!bookingVM.TableId.HasValue)
                    return (true, "Please select a table.");

                var table = _tableService.GetById(bookingVM.TableId.Value);
                if (table == null || !table.IsActive)
                    return (true, "Selected table does not exist or is inactive.");

                // التحقق من عدد الضيوف
                if (bookingVM.NumberOfGuests > table.Capacity)
                    return (true, $"Number of guests exceeds table capacity ({table.Capacity}).");

                // ✅ التحقق من عدم وجود حجز متداخل في نفس اليوم
                var overlappingBooking = _bookingRepo.GetAll(b =>
                    b.TableId == table.Id &&
                    b.BookingDate.Date == bookingVM.BookingDate.Date && // 👈 لازم نفس اليوم
                    (
                        (bookingVM.StartTime >= b.StartTime && bookingVM.StartTime < b.EndTime) ||
                        (bookingVM.EndTime > b.StartTime && bookingVM.EndTime <= b.EndTime) ||
                        (bookingVM.StartTime <= b.StartTime && bookingVM.EndTime >= b.EndTime)
                    )
                ).Any();

                if (overlappingBooking)
                    return (true, "This table is already booked for the selected date and time.");

                // إنشاء الحجز
                var newBooking = new Booking(
                    bookingVM.BookingDate,
                    bookingVM.StartTime,
                    bookingVM.EndTime,
                    bookingVM.NumberOfGuests,
                    bookingVM.Status,
                    bookingVM.SpecialRequests,
                    bookingVM.CustomerId,
                    bookingVM.TableId.Value
                );

                var result = _bookingRepo.Create(newBooking);

                return result ? (false, null) : (true, "Failed to create booking.");
            }
            catch (Exception ex)
            {
                return (true, ex.Message);
            }
        }


        //============================================================
        public bool Delete(int bookingId)
        {
            try
            {
                var oldBook = _bookingRepo.GetById(bookingId);
                if (oldBook == null)
                    return false;

                var result = _bookingRepo.Delete(bookingId);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //============================================================
        public (bool, string) Edit(int bookingId, EditBookingVM editBookingVM)
        {
            try
            {
                var oldBook = _bookingRepo.GetById(bookingId);
                if (oldBook == null)
                    return (true, "Booking not found");

                // التحقق من اختيار الطاولة
                if (!editBookingVM.TableId.HasValue)
                    return (true, "Please select a table.");

                var table = _tableService.GetById(editBookingVM.TableId.Value);
                if (table == null || !table.IsActive)
                    return (true, "Selected table does not exist or is inactive.");

                // التحقق من عدد الضيوف
                if (editBookingVM.NumberOfGuests > table.Capacity)
                    return (true, $"Number of guests exceeds table capacity ({table.Capacity}).");

                // ✅ التحقق من عدم وجود حجز متداخل في نفس اليوم باستثناء الحجز الحالي
                var overlappingBooking = _bookingRepo.GetAll(b =>
                    b.TableId == table.Id &&
                    b.Id != bookingId && // استثناء الحجز الحالي
                    b.BookingDate.Date == editBookingVM.BookingDate.Date && // 👈 لازم نفس اليوم
                    (
                        (editBookingVM.StartTime >= b.StartTime && editBookingVM.StartTime < b.EndTime) ||
                        (editBookingVM.EndTime > b.StartTime && editBookingVM.EndTime <= b.EndTime) ||
                        (editBookingVM.StartTime <= b.StartTime && editBookingVM.EndTime >= b.EndTime)
                    )
                ).Any();

                if (overlappingBooking)
                    return (true, "This table is already booked for the selected date and time.");

                // إنشاء الحجز الجديد للتعديل
                var newBook = new Booking(
                    editBookingVM.BookingDate,
                    editBookingVM.StartTime,
                    editBookingVM.EndTime,
                    editBookingVM.NumberOfGuests,
                    editBookingVM.Status,
                    editBookingVM.SpecialRequests,
                    editBookingVM.CustomerId,
                    editBookingVM.TableId.Value
                );

                var result = _bookingRepo.Edit(bookingId, newBook);

                return result
                    ? (false, "Booking updated successfully")
                    : (true, "Failed to edit booking");
            }
            catch (Exception ex)
            {
                return (true, ex.Message);
            }
        }


        //============================================================
        public (bool, string, List<GetAllBookingVM>) GetAll()
        {
            try
            {
                var bookings = _bookingRepo.GetAll(b => b.IsDeleted == null || b.IsDeleted == false);

                var result = bookings.Select(item => new GetAllBookingVM
                {
                    Id = item.Id,
                    BookingDate = item.BookingDate,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    NumberOfGuests = item.NumberOfGuests,
                    Status = item.Status,
                    SpecialRequests = item.SpecialRequests,
                    CustomerId = item.CustomerId,
                    TableId = item.TableId
                }).ToList();

                return (true, "Success", result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        //============================================================
        public (bool, string, EditBookingVM) GetById(int bookingId)
        {
            try
            {
                var booking = _bookingRepo.GetById(bookingId);

                if (booking == null)
                    return (true, "Booking not found", null);

                var result = new EditBookingVM
                {
                    BookingDate = booking.BookingDate,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    NumberOfGuests = booking.NumberOfGuests,
                    Status = booking.Status,
                    SpecialRequests = booking.SpecialRequests,
                    CustomerId = booking.CustomerId,
                    TableId = booking.TableId
                };

                return (false, "Success", result);
            }
            catch (Exception ex)
            {
                return (true, ex.Message, null);
            }
        }

        public (bool, string, List<GetAllBookingVM>) GetByCustomerId(int customerId)
        {
            try
            {
                var bookings = _bookingRepo.GetAll(b => b.CustomerId == customerId && (b.IsDeleted == null || b.IsDeleted == false));

                var result = bookings.Select(b => new GetAllBookingVM
                {
                    Id = b.Id,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    NumberOfGuests = b.NumberOfGuests,
                    Status = b.Status,
                    SpecialRequests = b.SpecialRequests,
                    TableId = b.TableId,
                    CustomerId = b.CustomerId
                }).ToList();

                return (true, "", result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, new List<GetAllBookingVM>());
            }
        }


        //============================================================
    }
}

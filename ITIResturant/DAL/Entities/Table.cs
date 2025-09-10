
namespace DAL.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Booking> Bookings { get; private set; } = new List<Booking>();

    }
}

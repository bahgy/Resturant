
namespace Restaurant.BLL.Model_VM.Table
{
    public class CreateTableVM
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class EditTableVM
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class GetAllTableVM
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }
}

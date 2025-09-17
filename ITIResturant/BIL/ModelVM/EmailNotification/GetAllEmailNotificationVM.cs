

namespace Restaurant.BLL.Model_VM.EmailNotification
{
    public class GetAllEmailNotificationVM
    {

        public int Id { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public DateTime SentTime { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string CustomerName { get; set; }
    }
}

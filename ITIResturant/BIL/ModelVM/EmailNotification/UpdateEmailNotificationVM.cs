

namespace Restaurant.BLL.Model_VM.EmailNotification
{
    public class UpdateEmailNotificationVM
    {
        public int Id { get; set; } // لازم نعرف أي Notification نعدلها
        public string ToAddress { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; }
    }
}

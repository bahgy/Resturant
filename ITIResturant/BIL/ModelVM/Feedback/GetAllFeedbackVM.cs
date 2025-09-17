

namespace Restaurant.BLL.Model_VM.Feedback
{
    public class GetAllFeedbackVM
    {

        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime SubmittedDate { get; set; }

        // عرض معلومات إضافية
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
    }
}

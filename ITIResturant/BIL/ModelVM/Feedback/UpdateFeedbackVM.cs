

namespace Restaurant.BLL.Model_VM.Feedback
{
    public class UpdateFeedbackVM
    {

        [Required(ErrorMessage = "Feedback Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }
        public DateTime SubmittedDate { get; internal set; }
    }
}

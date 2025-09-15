using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.Model_VM.EmailNotification
{
    public class CreateEmailNotificationVM
    {
        public string ToAddress { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Subject must be at least 5 characters long")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; }
        public string Type { get; set; }  // ممكن Enum أو String
        public int CustomerId { get; set; }
    }
}

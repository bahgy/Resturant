using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.Model_VM.EmailNotification
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

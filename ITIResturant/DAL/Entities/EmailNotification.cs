using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class EmailNotification
    {
        public int Id { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; } 
        public DateTime SentTime { get; set; }
        public string status {  get; set; }

        public string Type { get; set; } // Enum IS the best ?
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}

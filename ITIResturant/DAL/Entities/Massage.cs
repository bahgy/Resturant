using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Entities
{
    public class Message
    {

        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public int SenderID { get; set; }
        public AppUser Sender { get; set; }

        public int ReciverID { get; set; }
        public AppUser Reciver { get; set; }

        // NEW: associate message with an order (nullable if you want to keep user-to-user free messages)
        public int? OrderId { get; set; }
        public Order Order { get; set; }
    }
}

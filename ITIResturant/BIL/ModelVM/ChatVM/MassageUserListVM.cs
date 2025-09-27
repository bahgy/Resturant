using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.ModelVM.ChatVM
{
    public class MessageUserListVM
    {
        public int ID { get; set; }          // partner user id (delivery or customer)
        public string Username { get; set; }
        public string LastMessage { get; set; }
        public int OrderId { get; set; }     // NEW
        public string? OrderStatus { get; set; }
    }
}

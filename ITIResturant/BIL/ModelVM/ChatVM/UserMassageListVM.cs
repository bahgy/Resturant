using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.ModelVM.ChatVM
{
    public class UserMessageListVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public string time { get; set; }
        public bool IsCurrentUserSentMessage { get; set; }
    }
}

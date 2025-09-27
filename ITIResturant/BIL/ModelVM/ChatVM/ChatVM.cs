
namespace Restaurant.BLL.ModelVM.ChatVM
{
    public class ChatVM
    {
        public ChatVM()
        {
            Messages = new List<UserMessageListVM>();
        }
        public int CurrentUserId { get; set; }
        public int RecieverId { get; set; }
        public string RecieverUserName { get; set; }
        public int OrderId { get; set; }    // NEW
        public List<UserMessageListVM> Messages { get; set; }
    }
}

using Microsoft.AspNetCore.SignalR;

namespace Restaurant.PL.chatHub
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Support order-based chat
        public async Task SendMessage(int receiverId, string text, int? orderId = null)
        {
            var senderId = int.Parse(Context.UserIdentifier!);

            // Use orderId-aware service method
            var (hasError, message) = await _messageService.SendMessage(receiverId, text, orderId);
            if (hasError) return;

            var date = DateTime.Now.ToShortDateString();
            var time = DateTime.Now.ToShortTimeString();

            // Notify both sender and receiver
            await Clients.Users(senderId.ToString(), receiverId.ToString())
                .SendAsync("ReceiveMessage", text, date, time, senderId, orderId);
        }
    }
}

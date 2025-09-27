using Microsoft.AspNetCore.Http;
using Restaurant.BLL.ModelVM.ChatVM;
using Restaurant.BLL.Service.Abstraction;
using Restaurant.DAL.Repo.Abstraction;

namespace Restaurant.BLL.Service.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IChatRepo _chatRepo;
        private readonly IUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessageService(
            IChatRepo chatRepo,
            IUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _chatRepo = chatRepo;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        // -------------------- Customer Chats --------------------
        public async Task<(bool hasError, string? message, IEnumerable<MessageUserListVM>? users)> GetChatsForCustomer()
        {
            var currentUserId = _currentUserService.GetCurrentCustomerId(_httpContextAccessor.HttpContext.User);
            if (currentUserId == null) return (true, "User not logged in", null);

            var orders = await _chatRepo.GetOrdersWithDeliveryForCustomerAsync(currentUserId.Value);
            if (!orders.Any()) return (true, "No chats found", null);

            var result = new List<MessageUserListVM>();
            foreach (var o in orders)
            {
                var lastMsg = await _chatRepo.GetLastMessageByOrderAsync(o.Id);
                result.Add(new MessageUserListVM
                {
                    ID = o.Delivery.Id,
                    Username = o.Delivery.UserName,
                    LastMessage = lastMsg?.Text ?? "",
                    OrderId = o.Id,
                    OrderStatus = o.Status.ToString()
                });
            }
            return (false, null, result);
        }

        // -------------------- Delivery Chats --------------------
        public async Task<(bool hasError, string? message, IEnumerable<MessageUserListVM>? users)> GetChatsForDelivery()
        {
            var currentDeliveryId = _currentUserService.GetCurrentDeliveryId(_httpContextAccessor.HttpContext.User);
            if (currentDeliveryId == null) return (true, "User not logged in", null);

            var orders = await _chatRepo.GetOrdersForDeliveryAsync(currentDeliveryId.Value);
            if (!orders.Any()) return (true, "No chats found", null);

            var result = new List<MessageUserListVM>();
            foreach (var o in orders)
            {
                var lastMsg = await _chatRepo.GetLastMessageByOrderAsync(o.Id);
                result.Add(new MessageUserListVM
                {
                    ID = o.Customer.Id,
                    Username = o.Customer.UserName,
                    LastMessage = lastMsg?.Text ?? "",
                    OrderId = o.Id,
                    OrderStatus = o.Status.ToString()
                });
            }
            return (false, null, result);
        }

        // -------------------- Chat by Order --------------------
        public async Task<(bool hasError, string? message, ChatVM? chat)> GetMessageByOrder(int orderId)
        {
            var currentUserId = _currentUserService.GetCurrentCustomerId(_httpContextAccessor.HttpContext.User)
                                ?? _currentUserService.GetCurrentDeliveryId(_httpContextAccessor.HttpContext.User);

            if (currentUserId == null) return (true, "User not logged in", null);

            var order = await _chatRepo.GetOrderByIdAsync(orderId);
            if (order == null) return (true, "Order not found", null);

            // security: only customer or assigned delivery can open this chat
            if (order.CustomerId != currentUserId && order.DeliveryId != currentUserId)
                return (true, "Not authorized to access this chat", null);

            var conversation = await _chatRepo.GetConversationByOrderAsync(orderId);

            var chatVm = new ChatVM
            {
                CurrentUserId = currentUserId.Value,
                RecieverId = (order.CustomerId == currentUserId ? order.DeliveryId.Value : order.CustomerId),
                RecieverUserName = (order.CustomerId == currentUserId ? order.Delivery.UserName : order.Customer.UserName),
                OrderId = orderId,
                Messages = conversation.Select(i => new UserMessageListVM
                {
                    Id = i.Id,
                    Text = i.Text,
                    Date = i.Date.ToShortDateString(),
                    time = i.Date.ToShortTimeString(),
                    IsCurrentUserSentMessage = i.SenderID == currentUserId.Value
                }).ToList()
            };

            return (false, null, chatVm);
        }

        // -------------------- Send Message (NEW UNIFIED) --------------------
        public async Task<(bool hasError, string? message)> SendMessage(int receiverId, string text, int? orderId = null)
        {
            var currentUserId = _currentUserService.GetCurrentCustomerId(_httpContextAccessor.HttpContext.User)
                                ?? _currentUserService.GetCurrentDeliveryId(_httpContextAccessor.HttpContext.User);
            if (currentUserId == null) return (true, "User not logged in");

            if (string.IsNullOrWhiteSpace(text))
                return (true, "Message cannot be empty");

            // Order-based chat
            if (orderId.HasValue)
            {
                var order = await _chatRepo.GetOrderByIdAsync(orderId.Value);
                if (order == null) return (true, "Order not found");

                if (order.CustomerId != currentUserId && order.DeliveryId != currentUserId)
                    return (true, "Not authorized to send messages in this chat");

                if (order.CustomerId != receiverId && order.DeliveryId != receiverId)
                    return (true, "Receiver not part of this order");
            }

            // Save message
            await _chatRepo.SendMessageAsync(currentUserId.Value, receiverId, text, orderId);
            return (false, null);
        }

    }
}

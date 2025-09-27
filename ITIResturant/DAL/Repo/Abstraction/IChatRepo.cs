using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IChatRepo
    {
        // User related
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<IEnumerable<AppUser>> GetAllUsersExceptAsync(int currentUserId);

        // Old user-to-user chat (keep if still needed)
        Task<IEnumerable<Message>> GetConversationAsync(int currentUserId, int selectedUserId);
        Task<Message?> GetLastMessageAsync(int currentUserId, int otherUserId);

        // Sending message (with optional orderId)
        Task<Message> SendMessageAsync(int senderId, int receiverId, string text, int? orderId = null);

        // NEW: Order-based conversation
        Task<IEnumerable<Message>> GetConversationByOrderAsync(int orderId);
        Task<Message?> GetLastMessageByOrderAsync(int orderId);

        // NEW: Order helpers
        Task<IEnumerable<Order>> GetOrdersWithDeliveryForCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersForDeliveryAsync(int deliveryId);
        Task<Order?> GetOrderByIdAsync(int orderId);
    }
}

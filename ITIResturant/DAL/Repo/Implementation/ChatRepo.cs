
using Microsoft.EntityFrameworkCore;
using Restaurant.DAL.Repo.Abstraction;
using System.Security.AccessControl;

namespace Restaurant.DAL.Repo.Implementation
{
    public class ChatRepo : IChatRepo
    {
        private readonly RestaurantDbContext _dbContext;
        //private readonly IUserService _currentUserService;
        public ChatRepo(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersExceptAsync(int currentUserId)
        {
            return await _dbContext.Users
                .Where(x => x.Id != currentUserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(int currentUserId, int selectedUserId)
        {
            return await _dbContext.Messages
                .Where(m => (m.SenderID == currentUserId|| m.SenderID == selectedUserId) &&
                            (m.ReciverID == currentUserId|| m.ReciverID == selectedUserId))
                .OrderBy(m => m.Date)
                .ToListAsync();
        }

        public async Task<Message?> GetLastMessageAsync(int currentUserId, int otherUserId)
        {
            return await _dbContext.Messages
                .Where(m => (m.SenderID == currentUserId || m.SenderID == otherUserId) &&
                            (m.ReciverID == currentUserId || m.ReciverID == otherUserId))
                .OrderByDescending(m => m.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<Message> SendMessageAsync(int senderId, int receiverId, string text, int? orderId = null)
        {
            var message = new Message
            {
                SenderID = senderId,
                ReciverID = receiverId,
                Text = text,
                Date = DateTime.UtcNow,
                OrderId = orderId
            };
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
            return message;
        }
        public async Task<IEnumerable<Message>> GetConversationByOrderAsync(int orderId)
        {
            return await _dbContext.Messages
                .Where(m => m.OrderId == orderId)
                .OrderBy(m => m.Date)
                .ToListAsync();
        }

        public async Task<Message?> GetLastMessageByOrderAsync(int orderId)
        {
            return await _dbContext.Messages
                .Where(m => m.OrderId == orderId)
                .OrderByDescending(m => m.Date)
                .FirstOrDefaultAsync();
        }

        // Helpers to fetch orders relevant to a user:
        public async Task<IEnumerable<Order>> GetOrdersWithDeliveryForCustomerAsync(int customerId)
        {
            return await _dbContext.Orders
                .Include(o => o.Delivery)
                .Where(o => o.CustomerId == customerId && o.DeliveryId != null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersForDeliveryAsync(int deliveryId)
        {
            return await _dbContext.Orders
                .Include(o => o.Customer)
                .Where(o => o.DeliveryId == deliveryId)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.Delivery)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

    }
}

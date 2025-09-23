

namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IOrderRepo
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
        Task<bool> AddAsync(Order order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> UpdatePaymentStateAsync(int orderId, PaymentStatus paymentState);
    }
}

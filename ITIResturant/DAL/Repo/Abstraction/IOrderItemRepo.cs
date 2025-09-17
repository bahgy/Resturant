

namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IOrderItemRepo
    {

        Task<OrderItem> GetByIdAsync(int id);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetByProductIdAsync(int productId);
        Task<bool> AddAsync(OrderItem orderItem);
        Task<bool> UpdateAsync(OrderItem orderItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetTotalAmountByOrderIdAsync(int orderId);
    }
}

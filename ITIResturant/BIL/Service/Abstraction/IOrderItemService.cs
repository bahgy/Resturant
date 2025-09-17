

namespace Restaurant.BLL.Service.Abstraction
{
    public interface IOrderItemService
    {
        Task<OrderItemVM> GetByIdAsync(int id);
        Task<IEnumerable<OrderItemVM>> GetAllAsync();
        Task<IEnumerable<OrderItemVM>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItemVM>> GetByProductIdAsync(int productId);
        Task<bool> CreateAsync(CreateOrderItemVM viewModel);
        Task<bool> UpdateAsync(UpdateOrderItemVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
    }
}

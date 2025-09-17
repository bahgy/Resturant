
namespace Restaurant.BLL.Service.Abstraction
{
    public interface IOrderService
    {
        Task<OrderVM> GetByIdAsync(int id);
        Task<IEnumerable<OrderVM>> GetAllAsync();
        Task<IEnumerable<OrderVM>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<OrderVM>> GetByStatusAsync(string status);
        Task<OrderVM> CreateAsync(CreateOrderVM viewModel);
        Task<bool> UpdateAsync(UpdateOrderVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(OrderStatusUpdateVM viewModel);
        Task<bool> UpdatePaymentStateAsync(int orderId, string paymentState);
        Task<bool> ApplyPromoCodeAsync(int orderId, string promoCode);
        Task<bool> RemovePromoCodeAsync(int orderId);
    }
}

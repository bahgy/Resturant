
namespace Restaurant.BLL.Service.Abstraction
{
    public interface IOrderService
    {
        Task<(bool IsError, string ErrorMessage, OrderVM Data)> GetByIdAsync(int id);
        Task<(bool IsError, string ErrorMessage, IEnumerable<OrderVM> Data)> GetAllAsync();
        Task<(bool IsError, string ErrorMessage, IEnumerable<OrderVM> Data)> GetByCustomerIdAsync(int customerId);
        Task<(bool IsError, string ErrorMessage, IEnumerable<OrderVM> Data)> GetByStatusAsync(OrderStatus status);
        Task<(bool IsError, string ErrorMessage, OrderVM Data)> CreateAsync(CreateOrderVM viewModel);
        Task<(bool IsError, string ErrorMessage, bool Data)> UpdateAsync(UpdateOrderVM viewModel);
        Task<(bool IsError, string ErrorMessage, bool Data)> DeleteAsync(int id);
        Task<(bool IsError, string ErrorMessage, bool Data)> UpdateStatusAsync(OrderStatusUpdateVM viewModel);
        Task<(bool IsError, string ErrorMessage, bool Data)> UpdatePaymentStateAsync(int orderId, PaymentStatus paymentState);
        Task<(bool IsError, string ErrorMessage, bool Data)> ApplyPromoCodeAsync(int orderId, string promoCode);
        Task<(bool IsError, string ErrorMessage, bool Data)> RemovePromoCodeAsync(int orderId);
    }
}

using Resturant.BLL.ModelVM.PaymentVM;

namespace Resturant.BLL.Service.Abstraction
{
    public interface IPaymentService : IGenericService<Payment>
    {
        // Process a new payment for an order.
        Task<(bool error, string message, PaymentVM? payment)> ProcessPaymentAsync(OrderVM order, CreatePaymentVM createPaymentVM);

        // Update the status of a payment (Admin use).
        Task<(bool error, string message)> UpdatePaymentStatusAsync(EditPaymentVM editPaymentVM);

        // Get a payment by Id and return mapped VM.
        Task<(bool error, string message, PaymentVM? payment)> GetPaymentByIdAsync(int id);

        // Map a payment entity to its VM representation.
        PaymentVM MapPayment(Payment payment);
    }
}

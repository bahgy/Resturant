using Resturant.BLL.ModelVM.PaymentVM;
using Resturant.BLL.Service.Abstraction;
using Rsturant.DAL.Repo.Abstraction;

namespace Resturant.BLL.Service.Impelementation
{
    public class PaymentService : GenericService<Payment>, IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepo paymentRepo, IMapper mapper) : base(paymentRepo)
        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
        }

        // Process new payment for a given order.
        public async Task<(bool error, string message, PaymentVM? payment)> ProcessPaymentAsync(
    OrderVM order,
    CreatePaymentVM createPaymentVM)
        {
            try
            {
                var transactionId = Guid.NewGuid().ToString();

                var payment = new Payment
                {
                    OrderID = order.Id, // ✅ use correct property naming
                    Amount = createPaymentVM.Amount,
                    PayMethod = createPaymentVM.PaymentMethod, // ✅ matches entity
                    TransactionReference = transactionId,
                    IsSuccessful = true,
                    Status = PaymentStatus.Paid
                };

                await _paymentRepo.AddAsync(payment);
                await _paymentRepo.SaveAsync(); // ✅ Id will now be generated

                // Now payment.Id is set by the DB
                var vm = _mapper.Map<PaymentVM>(payment);

                return (false, "Payment processed successfully.", vm);
            }
            catch (Exception ex)
            {
                // you may also log this exception
                return (true, $"Payment processing failed: {ex.Message}", null);
            }
        }


        // Update payment status (Admin side).
        public async Task<(bool error, string message)> UpdatePaymentStatusAsync(EditPaymentVM editPaymentVM)
        {
            var payment = await _paymentRepo.GetByIdAsync(editPaymentVM.Id);
            if (payment == null)
                return (true, "Payment not found.");

            payment.Status = editPaymentVM.Status;

            await _paymentRepo.UpdateAsync(payment);
            await _paymentRepo.SaveAsync();

            return (false, "Payment status updated successfully.");
        }

        // Get payment by Id and map to VM.
        public async Task<(bool error, string message, PaymentVM? payment)> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            if (payment == null)
                return (true, "Payment not found.", null);

            return (false, "Payment retrieved successfully.", _mapper.Map<PaymentVM>(payment));
        }

        // Map payment entity to VM.
        public PaymentVM MapPayment(Payment payment)
        {
            return _mapper.Map<PaymentVM>(payment);
        }
    }
}

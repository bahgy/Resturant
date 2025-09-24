using AutoMapper;
using Restaurant.BLL.ModelVM.Payment;
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

       
        public async Task<Payment> ProcessPaymentAsync(Order order, decimal amount, PaymentMethod method)
        {
            var transactionId = Guid.NewGuid().ToString();

            var payment = new Payment
            {
                OrderID = order.Id,
                Amount = amount,
                Method = method,
                TransactionReference = transactionId,
                IsSuccsessful = true,
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepo.AddAsync(payment);
            await _paymentRepo.SaveAsync();

            return payment;
        }

       
        public PaymentVm MapPayment(Payment payment)
        {
            return _mapper.Map<PaymentVm>(payment);
        }
    }
}

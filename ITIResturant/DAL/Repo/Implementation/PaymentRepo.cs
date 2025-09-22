using Microsoft.EntityFrameworkCore;

using Rsturant.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rsturant.DAL.Repo.Impelementation
{
    public class PaymentRepo:GeniricRepo<Payment>,IPaymentRepo
    {
        private readonly RestaurantDbContext _context;

        public PaymentRepo(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionReference== transactionId);

            if (payment == null)
                throw new KeyNotFoundException($"No payment found with TransactionId: {transactionId}");

            return payment;
        }

    }
}

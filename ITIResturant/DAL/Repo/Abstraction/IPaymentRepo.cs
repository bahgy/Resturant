
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rsturant.DAL.Repo.Abstraction
{
    public interface IPaymentRepo:IGenericRepo<Payment>     
    {
        Task<Payment?> GetByTransactionIdAsync(string transactionId);
    }
}

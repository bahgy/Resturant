
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.BLL.Service.Abstraction
{
   public interface IPaymentService:IGenericService<Payment>
    {
        Task<Payment> ProcessPaymentAsync(Order order, decimal amount, PaymentMethod method);
    }
}

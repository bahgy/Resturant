
namespace Rsturant.DAL.Repo.Abstraction
{
    public interface IPaymentRepo:IGenericRepo<Payment>     
    {
        Task<Payment?> GetByTransactionIdAsync(string transactionId);
    }
}

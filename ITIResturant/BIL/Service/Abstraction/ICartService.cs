
using Restaurant.BLL.ModelVM.CartVM;

namespace Restaurant.BLL.Service.Abstraction
{
    public interface ICartService
    {
        Task<CartVM> GetCartAsync(int customerId);

        Task<(bool success, string? message)> AddToCartAsync(int productId, int customerId, int quantity = 1);
        Task<(bool success, string? message)> UpdateQuantityAsync(int customerId, int productId, int quantity);
        Task<(bool success, string? message)> RemoveItemAsync(int customerId, int productId);

    }
}

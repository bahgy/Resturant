namespace Restaurant.DAL.Repo.Abstraction
{
    public interface ICartRepo
    {
        Task<Cart> GetCartByCustomerIdAsync(int customerId);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task SaveChangesAsync();
    }
}

namespace Restaurant.DAL.Repo.Implementation
{
    public class CartRepo : ICartRepo
    {

        private readonly RestaurantDbContext _context;

        public CartRepo(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByCustomerIdAsync(int customerId)
        {
            return await _context.Carts
                .Include(c => c.ShopingCartItem)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}


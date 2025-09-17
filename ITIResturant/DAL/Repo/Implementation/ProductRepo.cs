
namespace Restaurant.DAL.Repo.Implementation
{
    public class ProductRepo:IProductRepo
    {
      
            private readonly RestaurantDbContext _context;

            public ProductRepo(RestaurantDbContext context)
            {
                _context = context;
            }

            public async Task<Product> GetByIdAsync(int id)
            {
                return await _context.Products.FindAsync(id);
            }

            public async Task<bool> ExistsAsync(int id)
            {
                return await _context.Products.AnyAsync(p => p.Id == id);
            }
        
    }
}

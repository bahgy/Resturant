
namespace Restaurant.DAL.Repo.Implementation
{
    public class ProductRepo:IProductRepo
    {
      
            private readonly RestaurantDbContext _context;

            public ProductRepo(RestaurantDbContext context)
            {
                _context = context;
            }
            public async Task<bool> ExistsAsync(int id)
            {
                return await _context.Products.AnyAsync(p => p.Id == id);
            }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                                 .Include(p => p.Category) // eager load category
                                 .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

    }
}

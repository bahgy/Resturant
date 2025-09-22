using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repo.Implementation
{
    public class MenuRepo : IMenuRepo
    {
        private readonly RestaurantDbContext _context;

        public MenuRepo(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesWithProductsAsync()
        {
            return await _context.Categories
                .Include(c => c.Products.Where(p => p.IsActive))
                .ToListAsync();
        }
    }
}

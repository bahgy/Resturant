using Restaurant.DAL.Database;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Repos.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Restaurant.DAL.Repos.Implementation.ProductRepo;

namespace Restaurant.DAL.Repos.Implementation
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

using DAL.DataBase;
using DAL.Entities;
using DAL.Repos.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.Repos.Implementation.ProductRepo;

namespace DAL.Repos.Implementation
{
    public class ProductRepo:IProductRepo
    {
      
            private readonly ResturantDbContext _context;

            public ProductRepo(ResturantDbContext context)
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

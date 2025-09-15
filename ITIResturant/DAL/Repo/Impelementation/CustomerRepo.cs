using DAL.DataBase;
using DAL.Entities;
using DAL.Repos.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos.Implementation
{
    public class CustomerRepo:ICustomerRepo
    {
        private readonly ResturantDbContext _context;

        public CustomerRepo(ResturantDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id);
        }

    }
}
 

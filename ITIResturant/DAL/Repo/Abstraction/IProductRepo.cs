using Restaurant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repos.Abstraction
{
    public interface IProductRepo
    {
        Task<Product> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

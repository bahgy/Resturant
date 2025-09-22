using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IMenuRepo
    {
        Task<List<Category>> GetAllCategoriesWithProductsAsync();
    }
}

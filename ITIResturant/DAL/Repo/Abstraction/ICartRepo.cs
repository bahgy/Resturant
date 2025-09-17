using Restaurant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repo.Abstraction
{
    public interface ICartRepo
    {
        IEnumerable<Cart> GetAll();
        Cart GetById(int id);
        void Add(Cart cart);
        void Update(Cart cart);
        void Delete(int id);
        void Save();
    }
}

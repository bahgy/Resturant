using Restaurant.BLL.Model_VM.Cart;
using Restaurant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Service.Abstraction
{
    public interface ICartService
    {
        IEnumerable<CartVM> GetAll();
        CartVM GetById(int id);
        void Add(CartVM cartVM);
        void Update(CartVM cartVM);
        void Delete(int id);

    }
}

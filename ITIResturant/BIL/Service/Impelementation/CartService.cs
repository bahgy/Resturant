using Restaurant.BLL.Model_VM.Cart;
using Restaurant.BLL.Service.Abstraction;
using Restaurant.DAL.Database;
using Restaurant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Service.Impelementation
{
    public class CartService : ICartService
    {
        private readonly RestaurantDbContext _context;

        public CartService(RestaurantDbContext context)
        {
            _context = context;
        }

        public void Add(CartVM cartVM)
        {
            
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CartVM> GetAll()
        {
            throw new NotImplementedException();
        }

        public CartVM GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(CartVM cartVM)
        {
            throw new NotImplementedException();
        }
    }
}

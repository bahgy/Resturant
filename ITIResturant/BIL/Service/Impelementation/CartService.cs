using BIL.Model_VM.Cart;
using BIL.Service.Abstraction;
using DAL.DataBase;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.Service.Impelementation
{
    public class CartService : ICartService
    {
        private readonly ResturantDbContext _context;

        public CartRepo(ResturantDbContext context)
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

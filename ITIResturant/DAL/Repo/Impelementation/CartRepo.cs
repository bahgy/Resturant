using DAL.DataBase;
using DAL.Entities;
using DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo.Impelementation
{
    public class CartRepo : ICartRepo
    {

        private readonly ResturantDbContext _context;

        public CartRepo(ResturantDbContext context)
        {
            _context = context;
        }
        //======================================================
        public void Add(Cart cart)
        {
            try
            {
                _context.Carts.Add(cart);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new cart.", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var cart = _context.Carts.Find(id);
                if (cart != null)
                    _context.Carts.Remove(cart);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting cart with Id = {id}.", ex);
            }
        }

        public IEnumerable<Cart> GetAll()
        {
            try
            {
                return _context.Carts
                    .Include(c => c.Customer)
                    .Include(c => c.ShopingCartItem)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching carts.", ex);
            }
        }

        public Cart GetById(int id)
        {
            try
            {
                return _context.Carts
                    .Include(c => c.Customer)
                    .Include(c => c.ShopingCartItem)
                    .FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching cart with Id = {id}.", ex);
            }
        }



        public void Update(Cart cart)
        {
            try
            {
                _context.Carts.Update(cart);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating cart with Id = {cart.Id}.", ex);
            }

        }

              public void Save()
              {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving changes in CartRepository.", ex);
            }

        }
    }
    }


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
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly ResturantDbContext _context;

        public OrderItemRepo(ResturantDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> AddAsync(OrderItem orderItem)
        {
            try
            {
                await _context.OrderItems.AddAsync(orderItem);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var orderItem = await GetByIdAsync(id);
                if (orderItem == null)
                    return false;

                _context.OrderItems.Remove(orderItem);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.OrderItems.AnyAsync(oi => oi.Id == id);
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .ToListAsync();
        }

        public async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByProductIdAsync(int productId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .Where(oi => oi.ProductId == productId)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalAmountByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .SumAsync(oi => oi.Price * oi.Quantity);
        }

        public async Task<bool> UpdateAsync(OrderItem orderItem)
        {
            try
            {
                _context.OrderItems.Update(orderItem);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}

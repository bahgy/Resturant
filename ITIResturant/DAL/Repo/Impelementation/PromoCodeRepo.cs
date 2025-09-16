using Restaurant.DAL.Database;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Repos.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repos.Implementation
{
    public class PromoCodeRepo:IPromoCodeRepo
    {
        private readonly RestaurantDbContext _context;

        public PromoCodeRepo(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<PromoCode> GetByIdAsync(int id)
        {
            return await _context.PromoCodes.FindAsync(id);
        }

        public async Task<PromoCode> GetByCodeAsync(string code)
        {
            return await _context.PromoCodes
                .FirstOrDefaultAsync(pc => pc.Code == code);
        }

        public async Task<IEnumerable<PromoCode>> GetAllAsync()
        {
            return await _context.PromoCodes.ToListAsync();
        }

        public async Task<IEnumerable<PromoCode>> GetValidPromoCodesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.PromoCodes
                .Where(pc => pc.ValidFromTime <= now && pc.ValidTo >= now && pc.UsedCount < pc.MaxUsedtime)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(PromoCode promoCode)
        {
            try
            {
                await _context.PromoCodes.AddAsync(promoCode);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(PromoCode promoCode)
        {
            try
            {
                _context.PromoCodes.Update(promoCode);
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
                var promoCode = await GetByIdAsync(id);
                if (promoCode == null)
                    return false;

                _context.PromoCodes.Remove(promoCode);
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
            return await _context.PromoCodes.AnyAsync(pc => pc.Id == id);
        }

        public async Task<bool> IsCodeValidAsync(string code)
        {
            var now = DateTime.UtcNow;
            return await _context.PromoCodes
                .AnyAsync(pc => pc.Code == code &&
                               pc.ValidFromTime <= now &&
                               pc.ValidTo >= now &&
                               pc.UsedCount < pc.MaxUsedtime);
        }

        public async Task<bool> IncrementUsageAsync(string code)
        {
            try
            {
                var promoCode = await GetByCodeAsync(code);
                if (promoCode == null)
                    return false;

                promoCode.UsedCount++;
                _context.PromoCodes.Update(promoCode);
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


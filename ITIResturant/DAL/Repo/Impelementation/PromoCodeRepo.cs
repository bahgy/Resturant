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
    public class PromoCodeRepo:IPromoCodeRepo
    {
        private readonly ResturantDbContext _context;

        public PromoCodeRepo(ResturantDbContext context)
        {
            _context = context;
        }

        public async Task<PromoCode> GetByIdAsync(int id)
        {
            return await _context.promoCodes.FindAsync(id);
        }

        public async Task<PromoCode> GetByCodeAsync(string code)
        {
            return await _context.promoCodes
                .FirstOrDefaultAsync(pc => pc.Code == code);
        }

        public async Task<IEnumerable<PromoCode>> GetAllAsync()
        {
            return await _context.promoCodes.ToListAsync();
        }

        public async Task<IEnumerable<PromoCode>> GetValidPromoCodesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.promoCodes
                .Where(pc => pc.ValidFromTime <= now && pc.ValidTo >= now && pc.UsedCount < pc.MaxUsedtime)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(PromoCode promoCode)
        {
            try
            {
                await _context.promoCodes.AddAsync(promoCode);
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
                _context.promoCodes.Update(promoCode);
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

                _context.promoCodes.Remove(promoCode);
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
            return await _context.promoCodes.AnyAsync(pc => pc.Id == id);
        }

        public async Task<bool> IsCodeValidAsync(string code)
        {
            var now = DateTime.UtcNow;
            return await _context.promoCodes
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
                _context.promoCodes.Update(promoCode);
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


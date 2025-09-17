using Restaurant.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DAL.Repos.Abstraction
{
    public interface IPromoCodeRepo
    {
        Task<PromoCode> GetByIdAsync(int id);
        Task<PromoCode> GetByCodeAsync(string code);
        Task<IEnumerable<PromoCode>> GetAllAsync();
        Task<IEnumerable<PromoCode>> GetValidPromoCodesAsync();
        Task<bool> AddAsync(PromoCode promoCode);
        Task<bool> UpdateAsync(PromoCode promoCode);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsCodeValidAsync(string code);
        Task<bool> IncrementUsageAsync(string code);
    }
}



namespace Restaurant.BLL.Service.Abstraction
{
    public interface IPromoCodeService
    {
        Task<PromoCodeVM> GetByIdAsync(int id);
        Task<PromoCodeVM> GetByCodeAsync(string code);
        Task<IEnumerable<PromoCodeVM>> GetAllAsync();
        Task<IEnumerable<PromoCodeVM>> GetActivePromoCodesAsync();
        Task<PromoCodeValidationVM> ValidatePromoCodeAsync(string code, decimal orderAmount);
        Task<PromoCodeVM> CreateAsync(CreatePromoCodeVM viewModel);
        Task<bool> UpdateAsync(UpdatePromoCodeVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<bool> IncrementUsageAsync(string code);
        Task<bool> DecrementUsageAsync(string code);
        Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);
    }
}

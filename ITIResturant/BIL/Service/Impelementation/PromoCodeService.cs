using AutoMapper;
using Restaurant.BLL.ModelVM.PromoCode;
using Restaurant.BLL.Service.Abstraction;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Enum;
using Restaurant.DAL.Repos.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Service.Implementation
{
    public class PromoCodeService:IPromoCodeService
    {
        private readonly IPromoCodeRepo _promoCodeRepository;
        private readonly IMapper _mapper;

        public PromoCodeService(IPromoCodeRepo promoCodeRepository, IMapper mapper)
        {
            _promoCodeRepository = promoCodeRepository;
            _mapper = mapper;
        }

        public async Task<PromoCodeVM> GetByIdAsync(int id)
        {
            var promoCode = await _promoCodeRepository.GetByIdAsync(id);
            return _mapper.Map<PromoCodeVM>(promoCode);
        }

        public async Task<PromoCodeVM> GetByCodeAsync(string code)
        {
            var promoCode = await _promoCodeRepository.GetByCodeAsync(code);
            return _mapper.Map<PromoCodeVM>(promoCode);
        }

        public async Task<IEnumerable<PromoCodeVM>> GetAllAsync()
        {
            var promoCodes = await _promoCodeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PromoCodeVM>>(promoCodes);
        }

        public async Task<IEnumerable<PromoCodeVM>> GetActivePromoCodesAsync()
        {
            var promoCodes = await _promoCodeRepository.GetValidPromoCodesAsync();
            return _mapper.Map<IEnumerable<PromoCodeVM>>(promoCodes);
        }

        public async Task<PromoCodeValidationVM> ValidatePromoCodeAsync(string code, decimal orderAmount)
        {
            var result = new PromoCodeValidationVM { Code = code };

            var promoCode = await _promoCodeRepository.GetByCodeAsync(code);

            if (promoCode == null)
            {
                result.IsValid = false;
                result.ErrorMessage = "Promo code not found";
                return result;
            }

            var now = DateTime.UtcNow;
            if (now < promoCode.ValidFromTime)
            {
                result.IsValid = false;
                result.ErrorMessage = "Promo code is not yet valid";
                return result;
            }

            if (now > promoCode.ValidTo)
            {
                result.IsValid = false;
                result.ErrorMessage = "Promo code has expired";
                return result;
            }

            if (promoCode.UsedCount >= promoCode.MaxUsedtime)
            {
                result.IsValid = false;
                result.ErrorMessage = "Promo code has reached maximum usage";
                return result;
            }

            if (orderAmount < promoCode.MinimumOrderAmount)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Minimum order amount of {promoCode.MinimumOrderAmount:C} required";
                return result;
            }

            // Calculate discount amount
            decimal discountAmount = 0;
            if (promoCode.DiscountType == DiscountType.Percentage)
            {
                discountAmount = orderAmount * (promoCode.DiscountValue / 100);
            }
            else
            {
                discountAmount = promoCode.DiscountValue;
            }

            result.IsValid = true;
            result.DiscountAmount = discountAmount;
            result.DiscountDisplay = promoCode.DiscountType == DiscountType.Percentage
                ? $"{promoCode.DiscountValue}%"
                : $"{promoCode.DiscountValue:C}";
            result.ValidTo = promoCode.ValidTo;
            result.PromoCode = promoCode; // Set the PromoCode property

            return result;
        }

        public async Task<PromoCodeVM> CreateAsync(CreatePromoCodeVM viewModel)
        {
            // Check if code is unique
            if (!await IsCodeUniqueAsync(viewModel.Code))
            {
                throw new InvalidOperationException("Promo code already exists");
            }

            var promoCode = _mapper.Map<PromoCode>(viewModel);
            promoCode.UsedCount = 0;

            var created = await _promoCodeRepository.AddAsync(promoCode);
            if (!created) return null;

            return await GetByCodeAsync(viewModel.Code);
        }

        public async Task<bool> UpdateAsync(UpdatePromoCodeVM viewModel)
        {
            var existingPromoCode = await _promoCodeRepository.GetByIdAsync(viewModel.Id);
            if (existingPromoCode == null) return false;

            // Check if code is unique (excluding current record)
            if (!string.IsNullOrEmpty(viewModel.Code) &&
                !await IsCodeUniqueAsync(viewModel.Code, viewModel.Id))
            {
                throw new InvalidOperationException("Promo code already exists");
            }

            _mapper.Map(viewModel, existingPromoCode);
            return await _promoCodeRepository.UpdateAsync(existingPromoCode);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _promoCodeRepository.DeleteAsync(id);
        }

        public async Task<bool> IncrementUsageAsync(string code)
        {
            var promoCode = await _promoCodeRepository.GetByCodeAsync(code);
            if (promoCode == null) return false;

            promoCode.UsedCount++;
            return await _promoCodeRepository.UpdateAsync(promoCode);
        }

        public async Task<bool> DecrementUsageAsync(string code)
        {
            var promoCode = await _promoCodeRepository.GetByCodeAsync(code);
            if (promoCode == null || promoCode.UsedCount <= 0) return false;

            promoCode.UsedCount--;
            return await _promoCodeRepository.UpdateAsync(promoCode);
        }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            var existingCode = await _promoCodeRepository.GetByCodeAsync(code);

            if (existingCode == null) return true;
            if (excludeId.HasValue && existingCode.Id == excludeId.Value) return true;

            return false;
        }
    }
}

